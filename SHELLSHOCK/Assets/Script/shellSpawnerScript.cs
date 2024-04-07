using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject[] ammoList;
    float timer;
    [SerializeField] float interval;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            spawnAmmo(transform.GetChild(i).transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval) {
            spawnAmmo(transform.GetChild(Mathf.RoundToInt(Random.Range(0,transform.childCount - 1))).transform.position);
            timer = 0;
        }
    }

    public void spawnAmmo(Vector3 pos) {
        //roll
        float roll = Random.Range(1f, 100f);
        int result = 0;
        if (roll <= 60f)
        {
            result = 0; //buckshot
        }
        else if (roll <= 80f)
        {
            result = 1; //slug
        }
        else if (roll <= 90f)
        {
            result = 2; //flare
        }
        else if (roll <= 100f) {
            result = 3; //airblast
        }

        Instantiate(ammoList[result], pos, Quaternion.identity);
    }
}
