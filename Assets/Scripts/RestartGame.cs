﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void onRestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("restart");
    }
}
