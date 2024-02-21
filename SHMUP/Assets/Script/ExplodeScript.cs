using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void die() {
        Destroy(gameObject);
    }
}
