using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearArea : MonoBehaviour
{
    private float playerDistance;
    void Update()
    {
        Distance();
    }
    void Distance()
    {
        playerDistance = (transform.position - GameManager.Instance.player.transform.position).magnitude;
        if (playerDistance < 1.5f)
        {
            UIManager.Instance.PressG.SetActive(true);
            UIManager.Instance.PressG.GetComponentInChildren<TextMeshProUGUI>().text = "G키를 누르면 다음회차를 진행합니다.";
            if (Input.GetKeyDown(KeyCode.G))
            {
                UIManager.Instance.PressG.SetActive(false);
                SceneManager.LoadSceneAsync("GameLoad");
            }
        }
        else
        {
            UIManager.Instance.PressG.SetActive(false);
        }
    }
}
