using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlaySceneManager : MonoBehaviour
{
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("GameStart");
    }
    public void KeySetting()
    {
        UIManager.Instance.keySettingImage.SetActive(true);
    }
    public void OnGameQuit()
    {
        Application.Quit();
    }
}
