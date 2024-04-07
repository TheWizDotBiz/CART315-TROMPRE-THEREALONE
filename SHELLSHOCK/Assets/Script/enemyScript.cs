using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    float interval = 0.1f;
    float timer;
    Collider col;
    Rigidbody rb;
    GameObject Player;
    bool noForward = false;
    bool noLeft = false;
    bool noRight = false;
    bool lastNoHorizontalDirectionWasLeft = false;
    float raycastDist = 2f;
    public float speed;
    float lateralSpeedMultiplier = 3f;
    [SerializeField] private int HP;
    bool hitByAirblast = false;
    float airblastDuration = 5f;
    [SerializeField] GameObject eyeR;
    [SerializeField] GameObject eyeL;
    [SerializeField] private float eyeLightDistance;
    ParticleSystem part;
    [SerializeField] GameObject gib;
    shellSpawnerScript SSS;
    public float speedIncrement = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        part = GetComponent<ParticleSystem>();
        SSS = GameObject.Find("ShellSpawners").GetComponent<shellSpawnerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!hitByAirblast)
        {
            
            if (timer >= interval)
            {
                determineDirection();
                timer = 0;
            }
        }
        else {
            if (timer >= airblastDuration) {
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ;
                timer = 0;
                hitByAirblast = false;
            }
        }
        
    }

    void determineDirection() {
        if (!hitByAirblast) {
            transform.LookAt(Player.transform.position);
            RaycastHit hitforward;
            if (Physics.Raycast(transform.position, transform.forward, out hitforward, raycastDist))
            {
                if (hitforward.transform.tag != "Player")
                {
                    print("ai forward raycast collided with " + hitforward.transform.name);
                    noForward = true;
                }
                else if (hitforward.transform.gameObject != gameObject)
                {
                    noForward = false;
                }
                else
                {
                    print("ai raycast is hitting itself, grand");
                }
            }
            else
            {
                noForward = false;
            }

            applyforce();
        }  
    }

    void applyforce()
    {
        Vector3 direction = transform.forward;
        float mySpeed = speed;
        if (noForward)
        {
            if (lastNoHorizontalDirectionWasLeft)
            {
                if (noRight)
                {
                    lastNoHorizontalDirectionWasLeft = false;
                    direction = transform.right * -1f;
                    mySpeed = speed * lateralSpeedMultiplier;
                }
                else {
                    direction = transform.right;
                    mySpeed = speed * lateralSpeedMultiplier;
                }

            }
            else {
                if (noLeft)
                {
                    lastNoHorizontalDirectionWasLeft = true;
                    direction = transform.right;
                    mySpeed = speed * lateralSpeedMultiplier;
                }
                else {
                    direction = transform.right * -1f;
                    mySpeed = speed * lateralSpeedMultiplier;
                }
            }
        }
        else {
            direction = transform.forward;
        }
        //apply velocity
        rb.velocity = direction * mySpeed;
        //eye lights
        if (Vector3.Distance(gameObject.transform.position, Player.transform.position) <= eyeLightDistance)
        {
            eyeR.SetActive(true);
            eyeL.SetActive(true);
        }
        else {
            eyeR.SetActive(false);
            eyeL.SetActive(false);
        }
        //speed incrementation
        speed += (speed * speedIncrement) * Time.deltaTime;
        //print("noForward: " + noForward + "noRight: " + noRight + "noLeft:" + noLeft + "lastNoHorizontalDIrectionWasLeft: " + lastNoHorizontalDirectionWasLeft);
       // rb.AddForce(direction * speed);
    }

    void damage() {
        part.Play();
        Instantiate(gib, transform.position, Quaternion.identity);
        HP--;
        if (HP <= 0) {
            rollForLoot();
            Destroy(gameObject);
        }
    }

    void kill() {
        part.Play();
        Instantiate(gib, transform.position, Quaternion.identity);
        rollForLoot();
        Destroy(gameObject);
    }

    public void airblastHit() {
        rb.velocity = Vector3.zero;
        hitByAirblast = true;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Camera.main.transform.forward * 1000f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player") {
            if (GameObject.Find("EndScreen") == null) {
                other.transform.GetComponent<AmmoScript>().die();
            }
        }
    }

    void rollForLoot() {
        if (Random.Range(0, 100f) <= 30f) {
            SSS.spawnAmmo(transform.position);
        }
    }
}
