using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzleFlashScript : MonoBehaviour
{
    Light myLight;
    float falloff = 10f;
    [SerializeField] Color[] colorList;
    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
        myLight.range = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (myLight.range > 0) {
            myLight.range -= falloff * Time.deltaTime;
            if (myLight.range <= 0) {
                myLight.range = 0;
            }
        }
    }

    public void lightMuzzle(int color) {
        myLight.color = colorList[color];
        myLight.range = 1f;
    }
}
