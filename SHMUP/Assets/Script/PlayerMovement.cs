using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float focusDivider;
    private float fireTimer;
    [SerializeField] private GameObject[] bulletList;
    [SerializeField] private float[] fireRateList;
    private int bulletIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //FIRERATE CONTROL
        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0) {
                fireTimer = 0;
            }
        }

        //CONTROL
        if (Input.GetKey("right")) {
            transform.position += new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("left")) {
            transform.position += new Vector3((horizontalSpeed * -1f) * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("up")) {
            transform.position += new Vector3(0, verticalSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey("down")) {
            transform.position += new Vector3(0, (-1f * verticalSpeed) * Time.deltaTime, 0);
        }

        if (Input.GetKeyDown("left shift")) {
            horizontalSpeed /= focusDivider;
            verticalSpeed /= focusDivider;
        }

        if (Input.GetKeyUp("left shift")) {
            horizontalSpeed *= focusDivider;
            verticalSpeed *= focusDivider;
        }

        //SCHUT
        if (Input.GetKey("z")) {
            if (fireTimer == 0) {
                Instantiate(bulletList[bulletIndex], transform.position, Quaternion.identity);
                fireTimer = fireRateList[bulletIndex];
            }
        }
    }
}
