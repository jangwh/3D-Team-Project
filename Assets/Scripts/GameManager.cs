using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player player;
    public PlayerLockOn playerLockOn;
    public GameObject PlayerPrefab;
    public Transform RespawnPos;
    public CinemachineVirtualCamera CinemachineVirtualCamera;

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
    void Start()
    {
        var spawnedPlayer = ObjectManager.Instance.SpawnPlayer(RespawnPos.position);
        SetPlayerReferences(spawnedPlayer);
        player = spawnedPlayer.GetComponent<Player>();
    }
    void Update()
    {
        Invoke("Revive", 1.5f);
    }
    void Revive()
    {
        if (player.isDie && Input.anyKeyDown)
        {
            UIManager.Instance.GameOver.SetActive(false);
            var spawnedPlayer = ObjectManager.Instance.SpawnPlayer(RespawnPos.position);
            SetPlayerReferences(spawnedPlayer);
            UIManager.Instance.frontHpBar.fillAmount = 1;
            UIManager.Instance.frontSteminaBar.fillAmount = 1;
            player.Init();
        }
    }
    void SetPlayerReferences(Player playerObj)
    {
        CinemachineVirtualCamera.Follow = playerObj.transform.Find("RPG-Character/Motion");
        CinemachineVirtualCamera.LookAt = playerObj.transform.Find("CameraTarget");

        player = playerObj.GetComponent<Player>();
        playerLockOn = playerObj.GetComponent<PlayerLockOn>();
        playerLockOn.virtualCamera = CinemachineVirtualCamera;
    }
}
