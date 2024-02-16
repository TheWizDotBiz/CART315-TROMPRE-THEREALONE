using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHolderScript : MonoBehaviour
{
    int readyToDestroyCount;

    public void readyToDestroy() {
        readyToDestroyCount++;
        if (readyToDestroyCount >= transform.childCount) {
            Destroy(gameObject);
        }
    }
}
