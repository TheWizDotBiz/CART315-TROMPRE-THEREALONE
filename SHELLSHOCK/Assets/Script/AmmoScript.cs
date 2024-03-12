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
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        shellName = GameObject.Find("ShellName").GetComponent<TextMeshProUGUI>();
        shellName.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        rotateUiShells(); //bad idea
        if (Input.GetMouseButtonDown(1)) { //right click, add spacebar later when jump is disabled
            CycleChamber();
        }
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

    void CycleChamber() { //what happens when you pump the shotgun
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
        }
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
            }
            print("picking up ammo");
            Destroy(other.gameObject);
            Vector3 newRot = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            UIshellRotations.Add(newRot);
            RenderShellUI();
        }
    }
}
