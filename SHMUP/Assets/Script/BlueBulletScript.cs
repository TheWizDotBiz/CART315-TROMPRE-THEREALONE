using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBulletScript : MonoBehaviour
{
    float speed = 10f;
    Collider2D col;
    float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
       // lifetime += Time.deltaTime;
        if (transform.position.y >= 5f) {
            transform.parent.GetComponent<BulletHolderScript>().readyToDestroy();
            gameObject.SetActive(false);
         //   Destroy(gameObject);
        }
    }
}
