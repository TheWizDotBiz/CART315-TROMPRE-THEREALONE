using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flareBulletScript : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    [SerializeField] float speed;
    bool collided = false;
    float lifetime = 10;
    float initLuminosity;
    Light myLight;
    // Start is called before the first frame update
    void Start()
    {
        myLight = transform.GetChild(0).GetComponent<Light>();
        initLuminosity = myLight.intensity;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(transform.forward * (speed * Time.deltaTime));
        if (!collided)
        {
            // rb.velocity = transform.forward * speed;
            rb.AddForce(transform.forward * (speed));
        }
        else {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0) {
                Destroy(gameObject);
            }
        }

        //light flicker
        myLight.intensity = initLuminosity + Random.Range(-1f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.name != "flareBullet(Clone)") {
            print("flarebullet collided with " + other.name + "which has tag " + other.tag);
            rb.velocity = Vector3.zero;
            switch (other.tag)
            {
                case "enemy":
                    other.gameObject.SendMessage("damage");
                    collided = true;
                    transform.SetParent(other.transform);
                    //rb.useGravity = true;
                    break;
                case "ammo":
                    other.GetComponent<Rigidbody>().AddForce(transform.forward * 25f);
                    collided = true;
                   // rb.useGravity = true;
                    break;
                default:
                    collided = true;
                    break;
            }
            transform.GetChild(0).transform.LookAt(GameObject.FindWithTag("Player").transform);
            transform.GetChild(0).transform.position += transform.GetChild(0).transform.forward * 3f;
            Destroy(col);
        }
        
    }
}
