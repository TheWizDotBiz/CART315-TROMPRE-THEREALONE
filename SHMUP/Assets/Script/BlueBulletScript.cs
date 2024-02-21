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
        Physics2D.IgnoreCollision(col, GameObject.Find("Hitbox").GetComponent<Collider2D>(), true);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("colliding with " + collision.transform.name);
        if (collision.transform.tag == "turret")
        {
            /*
            collision.transform.GetComponent<TurretScript>().HP--;
            if (collision.transform.GetComponent<TurretScript>().HP <= 0) {
                Destroy(collision.gameObject);
            }*/
            collision.transform.GetComponent<TurretScript>().takeDamage();
            transform.parent.GetComponent<BulletHolderScript>().readyToDestroy();
            gameObject.SetActive(false);
        }
    }
}
