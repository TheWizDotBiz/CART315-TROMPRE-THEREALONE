using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railgunScript : MonoBehaviour
{
    float timer;
    [SerializeField] float lifetime;
    [SerializeField] float speed;
    TrailRenderer tr;
    Color trailcolor;
    // Start is called before the first frame update
    private void Start()
    {
        tr = transform.GetChild(0).GetComponent<TrailRenderer>();
        trailcolor = tr.startColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
        tr.startColor = new Color(trailcolor.r, trailcolor.g, trailcolor.b, 1f - (timer / lifetime));
        tr.endColor = new Color(trailcolor.r, trailcolor.g, trailcolor.b, 1f - (timer / lifetime));
        timer += Time.deltaTime;
        if (timer >= lifetime) {
            Destroy(gameObject);
        }
    }
}
