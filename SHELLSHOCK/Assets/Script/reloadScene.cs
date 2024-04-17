using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class reloadScene : MonoBehaviour
{
    float fadeVal = 0;
    float fadespeed = 0.1f;
    bool active = false;
    [SerializeField] GameObject endscreen;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            SceneManager.LoadScene(1); //this is just because the fadeval shit isnt working rn so uhh fuck waiting 1.5 seconds
            fadeVal += Time.deltaTime * fadespeed;
            Camera.main.GetComponent<PSFXCamera>().Fade = fadeVal;
            if (fadeVal >= 1f)
            {
                SceneManager.LoadScene(1);
            }
        }
        else if (Input.GetKeyDown("r"))
        {
            if (endscreen.activeSelf)
            {
                active = true;
                endscreen.SetActive(false);
            }
        }
        else if (Input.GetKeyDown("b")) {
            SceneManager.LoadScene(0);
        }
    }
}
