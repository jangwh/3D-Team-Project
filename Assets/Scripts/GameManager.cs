using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player player;
    public GameObject PlayerPrefab;
    public Transform RespawnPos;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
    }
    void Update()
    {
        if(player.isDie && Input.anyKeyDown)
        {
            ObjectManager.Instance.SpawnPlayer(RespawnPos.position);
        }
    }

}
