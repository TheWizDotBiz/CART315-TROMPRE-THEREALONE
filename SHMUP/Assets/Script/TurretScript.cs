using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public int HP = 100;
    GameObject player;
    [SerializeField] GameObject bulletPrefab;
    private float timer;
    [SerializeField] private float fireRate;
    private float initFireRate;
    private int burstCount;
    private float colorTimer;
    private Color colorInit;
    private Quaternion initRot;
    [SerializeField] bool eyeDebug;
    [SerializeField] private GameObject explosion;
    private RestartScript rs;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        initFireRate = fireRate;
        colorInit = GetComponent<SpriteRenderer>().color;
        initRot = transform.rotation;
        rs = GameObject.Find("restarObject").GetComponent<RestartScript>();
    }

    // Update is called once per frame
    void Update()
    {

        //i ripped this from unity forums lol makes turret aim at player
        float angle = 0;

        Vector3 relative = transform.InverseTransformPoint(player.transform.position);
        angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, -angle);
        if (name == "Eye") {
            transform.rotation = initRot;
        }

        timer += Time.deltaTime;
        if (timer >= fireRate) {
            shoot();
            timer = 0;
        }

        if (colorTimer > 0) {
            colorTimer -= Time.deltaTime;
            if (colorTimer <= 0) {
                colorTimer = 0;
                GetComponent<SpriteRenderer>().color = colorInit;
            }
        }
    }

    void shoot() {
        switch (transform.name) {
            case "TurretOrange":
                for (int i = 0; i < 5; i++) {
                    GameObject thisBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    thisBullet.transform.Rotate(new Vector3(0, 0, (-30f + (15f * i))));
                }
                break;
            case "TurretTurq":
                GameObject turqBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                turqBullet.transform.Rotate(new Vector3(0, 0, -20f));
                break;
            case "TurretPurp":
                GameObject purpBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                burstCount++;
                if (burstCount < 7)
                {
                    fireRate = 0.2f;
                }
                else {
                    fireRate = initFireRate;
                    burstCount = 0;
                }
                break;
            case "TurretBlue":
                GameObject ringBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                break;
            case "Eye":
                if (transform.parent.childCount <= 1 || eyeDebug)
                {
                    GetComponent<Collider2D>().enabled = true;
                    int rand = Mathf.RoundToInt(Random.Range(0, 36f));
                    for (int i = 0; i < 36; i++)
                    {
                        if (i != rand)
                        {
                            GameObject eyeBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                            eyeBullet.transform.Rotate(new Vector3(0, 0, (-90f + (10f * i))) * -1f);
                        }
                    }
                }
                else {
                    print("childcount is " + transform.parent.childCount);
                }
                break;
            default:
              //  print("SHOOT METHOD ERROR IN SWITCH STATEMENT");
                break;
        }   
    }

    public void takeDamage() {
        HP--;
        colorTimer = 0.1f;
        GetComponent<SpriteRenderer>().color = Color.red;
        rs.updateHpBar();
        if (HP <= 0) {
            if (name == "Eye") {
                rs.gameOver(true);
                for (int i = 10; i < 10; i++) {
                    GameObject microExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
                    float randX = Random.Range(-5, 5f);
                    float randY = Random.Range(-5f, 5f);
                    float randS = Random.Range(0.5f, 5f);
                    microExplosion.transform.position += new Vector3(randX, randY, 0);
                    microExplosion.transform.localScale *= randS;
                }
            }
            GameObject thisExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            thisExplosion.transform.localScale *= 5f;
            Destroy(gameObject);
        }
    }
}
