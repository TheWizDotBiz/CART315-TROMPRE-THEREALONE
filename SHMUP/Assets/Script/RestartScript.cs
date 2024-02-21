using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
//This ended up being sort of a GameManager script actually
public class RestartScript : MonoBehaviour
{
    private float timer;
    [SerializeField] private GameObject MainText;
    [SerializeField] private GameObject restartText;
    [SerializeField] private GameObject HPBar;
    int maxHP;
    // Update is called once per frame
    private void Start()
    {
        MainText.SetActive(false);
        restartText.SetActive(false);
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("turret");
        foreach (GameObject thisTurret in turrets) {
            maxHP += thisTurret.GetComponent<TurretScript>().HP;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("r")) {
            SceneManager.LoadScene(0);
        }  
    }

    public void gameOver(bool win) {
        if (!win)
        {
            MainText.SetActive(true);
            restartText.SetActive(true);
        }
        else {
            MainText.SetActive(true);
            MainText.GetComponent<TextMeshProUGUI>().text = "YOU'VE HAVE WINNERED!!!!";
            restartText.SetActive(true);
        }
    }

    public void updateHpBar() {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("turret");
        float currentHP = 0;
        foreach (GameObject thisTurret in turrets) {
            currentHP += thisTurret.GetComponent<TurretScript>().HP;
        }
        float lerpval = currentHP / maxHP;
        print("lerpVal is " + lerpval + " resulting in value " + Mathf.Lerp(10f, 1910f, lerpval));
       
        HPBar.GetComponent<RectTransform>().offsetMax = new Vector2(Mathf.Lerp(1910f, 10f, lerpval) * -1f, HPBar.GetComponent<RectTransform>().offsetMax.y);
    }
}
