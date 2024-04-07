using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airblastSphereScript : MonoBehaviour
{
    Collider col;
    float timer;
    float lifetime = 0.1f;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (col.enabled == true) {
            timer += Time.deltaTime;
            if (timer >= lifetime) {
                timer = 0;
                col.enabled = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag) {
            case "enemy":
                other.transform.GetComponent<enemyScript>().airblastHit();
                break;
            case "ammo":
                other.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 100f);
                break;
        }
    }
}
