using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartSceneManager : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadSceneAsync("GameLoad");
    }
    public void OnGameQuit()
    {
        Application.Quit();
    }
}
