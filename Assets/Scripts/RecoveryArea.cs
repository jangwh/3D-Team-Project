using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecoveryArea : MonoBehaviour
{
    public MapMonsterSpawner spawner;
    
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
            UIManager.Instance.PressG.GetComponentInChildren<TextMeshProUGUI>().text = "G키를 누르면 회복됩니다.";
            if (Input.GetKeyDown(KeyCode.G))
            {
                spawner.DeactivateMonsters();
                GameManager.Instance.player.Init();
                UIManager.Instance.PressG.SetActive(false);
                spawner.ActivateMonsters();
            }
        }
        else
        {
            UIManager.Instance.PressG.SetActive(false);
        }
    }
}
