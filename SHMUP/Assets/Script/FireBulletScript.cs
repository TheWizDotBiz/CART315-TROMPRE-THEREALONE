using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletScript : MonoBehaviour
{
    private float lifetime;
    [SerializeField] private float lifetimeLimit;
    [SerializeField] private float speed;
    private bool purpBulletTriggered = false;
    private float purpBulletInitSpeed;
    private Vector3 ringBulletInitScale;
    private float ringtimer;
    // Start is called before the first frame update
    void Start()
    {
        purpBulletInitSpeed = speed;
        ringBulletInitScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.up * speed) * Time.deltaTime;
        
        lifetime += Time.deltaTime;
        if (lifetime >= lifetimeLimit) {
            Destroy(gameObject);
        }

        //unique bullet behaviours
        switch (transform.name) {
            case "GreenBullet(Clone)":
                transform.Rotate(new Vector3(0, 0, Mathf.Sin(lifetime * 5f) * 100f) * Time.deltaTime);
                break;
            case "PurpBullet(Clone)":
                if (lifetime <= lifetimeLimit / 2f)
                {
                    float index = lifetime / (lifetimeLimit / 2f);
                    speed = Mathf.Lerp(purpBulletInitSpeed, 0, index);
                }
                else {
                    if (!purpBulletTriggered) {
                        GameObject player = GameObject.Find("Player");
                        float angle = 0;

                        Vector3 relative = transform.InverseTransformPoint(player.transform.position);
                        angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
                        transform.Rotate(0, 0, -angle);
                        purpBulletTriggered = true;
                    }
                    speed = purpBulletInitSpeed * 2.5f;
                }
                break;
            case "RingBullet(Clone)":
                transform.localScale += new Vector3(1f, 1f, 0) * Time.deltaTime;
                ringtimer += Time.deltaTime;
                if (ringtimer >= 0.1f)
                {
                    GameObject player = GameObject.Find("Player");
                    float angle = 0;

                    Vector3 relative = transform.InverseTransformPoint(player.transform.position);
                    angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
                    transform.Rotate(0, 0, -angle);
                    ringtimer = 0;
                }
                break;
            case "EyeBullet(Clone)":
                transform.Rotate(new Vector3(0, 0, 10f * Time.deltaTime));
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Hitbox") {
            //Destroy(GameObject.Find("Player"));
            GameObject.Find("Player").SetActive(false);
        }
    }
}
