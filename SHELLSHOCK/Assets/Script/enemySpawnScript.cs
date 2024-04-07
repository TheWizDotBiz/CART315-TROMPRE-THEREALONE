using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnScript : MonoBehaviour
{
    [SerializeField] GameObject enemyObject;
    float timer;
    float interval = 1.5f;
    int difficulty = 2;
    int spawnedEnemies = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval) {
            if (spawnedEnemies < difficulty)
            {
                spawnEnemy();
            }
            else {
                if (GameObject.FindGameObjectsWithTag("enemy").Length == 0) {
                    difficulty += Mathf.RoundToInt(difficulty / 2f);
                    spawnedEnemies = 0;
                }
            }
            timer = 0;
        }
    }

    void spawnEnemy() {
        int roll = Mathf.RoundToInt(Random.Range(0, transform.childCount));
        GameObject newEnemy = Instantiate(enemyObject, transform.GetChild(roll).transform.position, Quaternion.identity);
        newEnemy.GetComponent<enemyScript>().speed += (difficulty / 10f);
        newEnemy.GetComponent<enemyScript>().speedIncrement += (difficulty / 100f);
        spawnedEnemies++;
    }
}
