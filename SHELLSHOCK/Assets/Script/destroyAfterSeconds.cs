using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterSeconds : MonoBehaviour
{
    float lifetime = 3f;
    

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }
}
