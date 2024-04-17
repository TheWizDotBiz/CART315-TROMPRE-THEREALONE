using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoScript : MonoBehaviour
{
    public List<int> ammoList = new List<int>();
    [SerializeField] private GameObject[] ammoPickupPrefabs;
    [SerializeField] private GameObject shellExtractPoint;
    [SerializeField] private float shellExtractStrength;
    [SerializeField] private float shellRotateSpeed;
    private Canvas canvas;
    private TextMeshProUGUI shellName;
    [SerializeField] private GameObject[] UIshells; //sync this up with int values for shells, ie ammoList value 0  is a buck shell and so forth
    [SerializeField] private string[] UIshellsNames;
    private Color[] UIshellNameColor = {Color.white, Color.red, Color.green, Color.yellow, Color.cyan, Color.magenta};
    List<GameObject> UIshellList = new List<GameObject>();
    List<Vector3> UIshellRotations = new List<Vector3>();
    // [SerializeField] private GameObject buckshot;
    // Start is called before the first frame update

    //shooting ig
    [SerializeField] private GameObject FiringPoint;
    [SerializeField] int buckShotPelletCount;
    [SerializeField] GameObject shotImpactPrefab;
    private Camera cam;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject railgunBullet;
    [SerializeField] private GameObject flareBullet;
    private PlayerControlRB prb;
    [SerializeField] private Collider airblastSphere;
    [SerializeField] private muzzleFlashScript muzzleflash;
    [SerializeField] GameObject endscreen;

    //audio
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip[] sounds; //0 shoot, //1 pump //2 pickup //3 die //4 dryfire

    //score
    int score = 0;
    int kills = 0;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI airtimeText;
    TextMeshProUGUI killsText;
    int lastScoreVal;
    float lastAirtimeVal;
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        shellName = GameObject.Find("ShellName").GetComponent<TextMeshProUGUI>();
        shellName.text = "";
        cam = Camera.main;
        prb = GetComponent<PlayerControlRB>();
        scoreText = GameObject.Find("scoreText").GetComponent<TextMeshProUGUI>();
        airtimeText = GameObject.Find("airtimeText").GetComponent<TextMeshProUGUI>();
        killsText = GameObject.Find("killsText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        rotateUiShells(); //bad idea //ig not
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("space")) { //right click, add spacebar later when jump is disabled
                anim.SetTrigger("Pump");
        }
        if (Input.GetMouseButtonDown(0)) {
            if (ammoList.Count > 0)
            {
                if (ammoList[0] != 0)
                {
                    anim.SetTrigger("Shoot");
                }
                else {
                    dryFire();
                }
                
            }
            else {
                dryFire();
            }
                
        }

        if (lastAirtimeVal != prb.airtime || lastScoreVal != score) {
            handleText();
        }
        lastAirtimeVal = prb.airtime;
        lastScoreVal = score;
    }

    //this is compatible with characterController collision, but is called every frame where there is collision, and there is no alternative, maybye adding a seaprate collider on player would be a better option?
    /* private void OnControllerColliderHit(ControllerColliderHit hit)
     {
         if (hit.transform.tag == "ammo")
         {
             print("picking up ammo");
             Destroy(hit.gameObject);
         }
     }*/
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ammo")
        {
            print("picking up ammo");
            Destroy(collision.gameObject);
        }
    }*/

    //this one works! as long as the other collider isnt a trigger and has a rigidbody
    void RenderShellUI() {
        List<Quaternion> tempQuat = new List<Quaternion>();
        for (int j = 0; j < UIshellList.Count; j++) {
            tempQuat.Add(UIshellList[j].transform.rotation);
            Destroy(UIshellList[j]);
        }
        UIshellList.Clear();
       // UIshellRotations.Clear();
        for (int i = 0; i < ammoList.Count; i++) {
            if (i == 0)
            {
               
                GameObject thisShell = Instantiate(UIshells[ammoList[i]], canvas.transform);
                if (i < tempQuat.Count)
                {
                    thisShell.transform.rotation = tempQuat[i];
                }
                else {
                    Quaternion thisRot = Quaternion.Euler(-90f, 0, 0);
                    thisShell.transform.rotation = thisRot;
                }
                thisShell.transform.localPosition = new Vector3(800f, -325f, 0);
                thisShell.transform.localScale *= 1.5f;
                UIshellList.Add(thisShell);
                shellName.text = UIshellsNames[ammoList[i]];
                shellName.color = UIshellNameColor[ammoList[i]];
               // shellName.color = Color.red;
            }
            else {
                GameObject thisShell = Instantiate(UIshells[ammoList[i]], canvas.transform);
                if (i < tempQuat.Count)
                {
                    thisShell.transform.rotation = tempQuat[i];
                }
                else
                {
                    Quaternion thisRot = Quaternion.Euler(-90f, 0, 0);
                    thisShell.transform.rotation = thisRot;
                }
                thisShell.transform.localPosition = new Vector3(755f - (140f * i), -400f, 0);
                UIshellList.Add(thisShell);
            }
            //   Quaternion newRot = Quaternion.Euler(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            /*
            Vector3 newRot = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            UIshellRotations.Add(newRot);*/
        }
    }

    void rotateUiShells() {
        for (int i = 0; i < UIshellList.Count; i++) {
            UIshellList[i].transform.Rotate((UIshellRotations[i] * shellRotateSpeed) * Time.deltaTime);
        }
    }

    public void CycleChamber() { //what happens when you pump the shotgun
        if (ammoList.Count > 0) {
            if (ammoList[0] != 0)
            {
                Quaternion thisRot = Quaternion.Euler(-90f, 0, 0);
                GameObject extractedShell = Instantiate(ammoPickupPrefabs[ammoList[0]], shellExtractPoint.transform.position, thisRot);
                extractedShell.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * shellExtractStrength);
                extractedShell.GetComponent<Rigidbody>().AddTorque(Camera.main.transform.forward * shellExtractStrength);
                ammoList[0] = 0;
                RenderShellUI();
            }
            ammoList.Remove(0);
            UIshellRotations.Remove(UIshellRotations[0]);
            RenderShellUI();
            audio.clip = sounds[1];
            audio.Play();
        }
    }

    public void shootShotgun() {
        if (ammoList.Count > 0)
        {
            if (ammoList[0] != 0)
            {
                bool isInfinite = false;
                switch (ammoList[0])
                {
                    case 1: //buckshot
                    case 5: //infinite
                        for (int i = 0; i < buckShotPelletCount; i++)
                        {
                            float xOffset = Random.Range(-0.075f, 0.075f);
                            float yOffset = Random.Range(-0.075f, 0.075f);
                            Vector3 direction = cam.transform.forward + new Vector3(xOffset, yOffset, 0f);
                            RaycastHit hit;
                            if (Physics.Raycast(cam.transform.position, direction, out hit, Mathf.Infinity))
                            {
                                switch (hit.transform.tag)
                                {
                                    case "enemy":
                                        hit.transform.SendMessage("damage");
                                        break;
                                    case "ammo":
                                        hit.transform.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 25f);
                                        break;
                                    default:
                                       // Instantiate(shotImpactPrefab, hit.point, Quaternion.identity); //put bullet hit here
                                        break;
                                }
                            }
                        }
                        applyBackdraft(12.5f);
                        if (ammoList[0] == 5)
                        {
                            isInfinite = true;
                            muzzleflash.lightMuzzle(4);
                        }
                        else {
                            muzzleflash.lightMuzzle(0);
                        }
                        break;
                    case 2: //slug
                        RaycastHit[] hitsRailgun = Physics.RaycastAll(cam.transform.position, cam.transform.forward, Mathf.Infinity);
                        Vector3 bulletVisualEndpoint = new Vector3(0,0,0);
                        for (int i = 0; i < hitsRailgun.Length; i++) {
                            switch (hitsRailgun[i].transform.tag) {
                                case "enemy":
                                    hitsRailgun[i].transform.SendMessage("kill");
                                    break;
                                case "ammo":
                                    hitsRailgun[i].transform.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 250f);
                                    break;
                                default:
                                 //   Instantiate(shotImpactPrefab, hitsRailgun[i].point, Quaternion.identity); //put slug impact here
                                    bulletVisualEndpoint = hitsRailgun[i].point;
                                    break;
                            }
                        }
                        GameObject shot = Instantiate(railgunBullet, FiringPoint.transform.position, Quaternion.identity);

                        shot.transform.LookAt(bulletVisualEndpoint);
                        applyBackdraft(15f);
                        muzzleflash.lightMuzzle(1);
                        //shot.transform.position = hitsRailgun[hitsRailgun.Length - 1].transform.position;
                        /*
                        if (Physics.RaycastAll(cam.transform.position, cam.transform.forward, out hitRailgun, Mathf.Infinity)) {
                            
                        }*/
                        break;
                    case 3:// flare
                        for (int f = 0; f < buckShotPelletCount; f++) {
                            float xOffset = Random.Range(-0.075f, 0.075f);
                            float yOffset = Random.Range(-0.075f, 0.075f);
                            Vector3 direction = cam.transform.forward + new Vector3(xOffset, yOffset, 0f);
                            RaycastHit hit;
                            if (Physics.Raycast(cam.transform.position, direction, out hit, Mathf.Infinity)) {
                                GameObject thisShot = Instantiate(flareBullet, FiringPoint.transform.position, cam.transform.rotation);
                                thisShot.transform.LookAt(hit.point);
                            }
                        }
                        applyBackdraft(10f);
                        muzzleflash.lightMuzzle(2);
                        break;
                    case 4: //airblast
                        airblastSphere.enabled = true;
                        applyBackdraft(20f);
                        muzzleflash.lightMuzzle(3);
                        break;
                    default:

                        break;
                }
                /*
                if (isInfinite) { //inifite shell that requries pumping
                    ammoList.Add(5);
                    Vector3 newRot = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    UIshellRotations.Add(newRot);
                }*/
                if (!isInfinite) { //infinite shell that does NOT require pumping
                    ammoList[0] = 0;
                }
                RenderShellUI();
                audio.clip = sounds[0];
                audio.Play();
            }
            else
            {
                dryFire();
            }
        }
        else {
            dryFire();
        }
        
    }

    void dryFire() {
        //print("Click! chamber is empty");
        audio.clip = sounds[4];
        audio.Play();
    }

    void applyBackdraft(float strength) {
        Vector3 direction = cam.transform.forward * strength;
        prb.backdraft(direction * -1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "ammo") {
            if (ammoList.Count < 8) {
                switch (other.transform.name) {
                    case "shell(clone)":
                    case "shell(Clone)": //this is the correct instantiation name nomenclature, reproduce this for further ammo objects.
                    case "shell":
                        ammoList.Add(1);
                        break;

                    

                    case "slug":
                    case "slug(Clone)":
                        ammoList.Add(2);
                        break;

                    case "flare":
                    case "flare(Clone)":
                        ammoList.Add(3);
                        break;

                    case "airblast":
                    case "airblast(Clone)":
                        ammoList.Add(4);
                        break;
                    case "infinite":
                    case "infinite(Clone)":
                        ammoList.Add(5);
                        break;
                    
                    default:
                        print("this shell is not lsited in the switch statement in AmmoScript.cs");

                        break;
                }
                print("picking up ammo");
                Destroy(other.gameObject);
                Vector3 newRot = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                UIshellRotations.Add(newRot);
                RenderShellUI();
                audio.clip = sounds[2];
                audio.Play();
            }
        }
    }

    public void die() {
        prb.enabled = false;
        //GetComponent<CharacterController>().enabled = false;
        Destroy(GetComponent<CharacterController>());
        GetComponent<Collider>().isTrigger = false;
        gameObject.AddComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.None;
        Destroy(muzzleflash.gameObject.transform.parent.parent.gameObject); //shotgun
        endscreen.SetActive(true);
        audio.clip = sounds[3];
        audio.Play();

        //record high scores
        if (PlayerPrefs.GetInt("HiScore") < score) {
            PlayerPrefs.SetInt("HiScore", score);
            endscreen.transform.GetChild(1).gameObject.SetActive(true);
            endscreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "NEW HIGH SCORE! : " + PlayerPrefs.GetInt("HiScore");
            //activate text for new hi score
        }
        if (PlayerPrefs.GetInt("HiKillcount") < kills) {
            PlayerPrefs.SetInt("HiKillcount", kills);
            endscreen.transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(true);
            endscreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "NEW TOP KILLS! :" + PlayerPrefs.GetInt("HiKillcount") + " DEAD KRAZY GUYZ";
        }
        if (PlayerPrefs.GetFloat("HiAirtime") < prb.highestAirtime) {
            PlayerPrefs.SetFloat("HiAirtime", prb.highestAirtime);
            endscreen.transform.GetChild(2).gameObject.SetActive(true);
            endscreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "NEW LONGEST AIRTIME! : " + Mathf.RoundToInt((100f * (1f + PlayerPrefs.GetFloat("HiAirtime"))) * (1f + (kills / 100f)));
        }
        //airtime hiscore
       // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void scorePoints() {
        kills++;
        score += Mathf.RoundToInt((100f * (1f + prb.airtime)) * (1f + (kills / 100f))); //the math is to round up to 2 decimals
    }

    public void handleText() {
        if (prb.airtime >= 0.1f) //this is because there are some flickering issues with the airTime on prb when grounded, we basically just dont display it if its not high enough
        {
            airtimeText.text = "AIRTIME: " + (Mathf.Round(prb.airtime * 100)) / 100.0 + "s";
        }
        else {
            airtimeText.text = "AIRTIME: 0s";
        }
       
        scoreText.text = "SCORE: " + score;
        killsText.text = "FRAGS: " + kills;
    }
}
