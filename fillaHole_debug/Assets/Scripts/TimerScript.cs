using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    private Text timerText;
    private float levelTime;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
        ResetTimer();
    }

    public void ResetTimer()
    {
        levelTime = 30;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (levelTime > 0)
        {
            levelTime -= Time.deltaTime;
            //this is so fucking unnecessary but fuck it i cant think of a better way lmaonade
            //tldr there is definetly a mathf callback or something to shorten something to a certain decimal count but i forgor and i cant google
            //so instead we just turn the float into a string, go through every character until we hit the dot an then only add X decimals to the string and then stop it short
            //where X is the amount of decimals after dot we want (i couldnt tell from where im standing but its 1 apparently)
            string timetext = levelTime.ToString();
            string newTimeText = "";
            bool dotfound = false;
            int numbersPastDot = 0;
            for (int i = 0; i < timetext.Length; i++) {
                if (timetext[i] == '.')
                {
                    dotfound = true;
                }

                if (!dotfound)
                {
                    newTimeText = newTimeText + timetext[i];
                }
                else {
                    if (numbersPastDot != 2)
                    {
                        newTimeText = newTimeText + timetext[i];
                        numbersPastDot++;
                    }
                    else {
                        i = timetext.Length; //stop for loop
                    }
                }
            }
            //end tomfuckery
            timerText.text = newTimeText + " SECONDS LEFT";
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
