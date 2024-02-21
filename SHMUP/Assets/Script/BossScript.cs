using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    private float hLerpIndex = 0.5f; //so it starts with it in the middle
    private bool hDir;
    [SerializeField] private float hSpeed;
    [SerializeField] private float vSpeed;
    private float yPos;
    private float initYPos;
    private float lifetime;
    //x 6.15
    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.position.y;
        initYPos = yPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0, rotateSpeed * Time.deltaTime));
        moveVertical();
        moveHorizontal();
    }

    void moveVertical() {
        lifetime += Time.deltaTime;
        yPos = (Mathf.Sin(lifetime) * vSpeed) + initYPos;
    }

    void moveHorizontal() {
        if (!hDir)
        {
            hLerpIndex += (Time.deltaTime * hSpeed) * (1.1f - hLerpIndex);
            if (hLerpIndex >= 1f)
            {
                hLerpIndex = 1f;
            }
            transform.position = Vector3.Lerp(new Vector3(-6.15f, yPos, 0), new Vector3(6.15f, yPos, 0), hLerpIndex);
            if (hLerpIndex == 1f) {
                hLerpIndex = 0;
                hDir = true;
            }
        }
        else {
            hLerpIndex += (Time.deltaTime * hSpeed) * (1.1f - hLerpIndex);
            if (hLerpIndex >= 1f)
            {
                hLerpIndex = 1f;
            }
            transform.position = Vector3.Lerp(new Vector3(6.15f, yPos, 0), new Vector3(-6.15f, yPos, 0), hLerpIndex);
            if (hLerpIndex == 1f) {
                hLerpIndex = 0f;
                hDir = false;
            }
        }
    }
}
