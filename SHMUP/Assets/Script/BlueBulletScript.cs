using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBulletScript : MonoBehaviour
{
    [SerializeField] float speedV;
    [SerializeField] float speedH;
    Collider2D col;
    float lifetime;
    [SerializeField] private GameObject explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(col, GameObject.Find("Hitbox").GetComponent<Collider2D>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speedH * Time.deltaTime, speedV * Time.deltaTime, 0);
       // lifetime += Time.deltaTime;
        if (transform.position.y >= 5f || transform.position.x < -9f || transform.position.x > 9f) {
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
            Instantiate(explosion, transform.position, Quaternion.identity);
            collision.transform.GetComponent<TurretScript>().takeDamage();
            transform.parent.GetComponent<BulletHolderScript>().readyToDestroy();
            gameObject.SetActive(false);
        }
    }
}
