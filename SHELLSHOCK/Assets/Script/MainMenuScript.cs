using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject hiScoreHolder;
    [SerializeField] private GameObject howToHolder;
    [SerializeField] private TextMeshProUGUI scoretext;
    [SerializeField] private TextMeshProUGUI airtimetext;
    [SerializeField] private TextMeshProUGUI killtext;

    [SerializeField] private Light pointlight;
    private float initIntensity; //its range but whatever
    private float initX;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        initIntensity = pointlight.range;
        initX = pointlight.gameObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        pointlight.range = initIntensity + Random.Range(-5f, 5f);
        pointlight.gameObject.transform.position = new Vector3((initX + Mathf.Sin(time)) * 5f, pointlight.transform.position.y, pointlight.transform.position.z);

        //control
        if (Input.GetKeyDown("p")) {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown("h")) {
            howToHolder.SetActive(!howToHolder.activeSelf);
            hiScoreHolder.SetActive(false);
        }

        if (Input.GetKeyDown("s")) {
            hiScoreHolder.SetActive(!hiScoreHolder.activeSelf);
            howToHolder.SetActive(false);
            scoretext.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HiScore");
            airtimetext.text = "TOP AIRTIME: " + (Mathf.Round(PlayerPrefs.GetFloat("HiAirtime") * 100)) / 100.0 + " SECONDS";
            killtext.text = "TOP FRAGZ: " + PlayerPrefs.GetInt("HiKillcount") + " DEaD BASTERDZ";
        }

        if (Input.GetKeyDown("q")) {
            Application.Quit();
        }

        if (Input.GetKeyDown("r")) {
            if (hiScoreHolder.activeSelf) {
                PlayerPrefs.SetInt("HiScore", 0);
                PlayerPrefs.SetFloat("HiAirtime", 0f);
                PlayerPrefs.SetInt("HiKillcount", 0);
            }
        }
    }
}
