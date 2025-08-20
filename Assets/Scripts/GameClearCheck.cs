using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearCheck : MonoBehaviour
{
    public GameObject clearspot;
    
    public void GameClear()
    {
        Instantiate(clearspot, transform.position, transform.rotation);
    }
}
