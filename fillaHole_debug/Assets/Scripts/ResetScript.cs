﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    public void Restart() {
        SceneManager.LoadScene("Main");
    }
}
