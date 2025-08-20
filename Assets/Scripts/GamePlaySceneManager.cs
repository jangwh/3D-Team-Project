using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlaySceneManager : MonoBehaviour
{
    public void OnGameReturn()
    {
        UIManager.Instance.ESCMenu.SetActive(false);
        UIManager.Instance.isESCMenuOn = false;
    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("GameStart");
    }
    public void OnGameQuit()
    {
        Application.Quit();
    }

}
