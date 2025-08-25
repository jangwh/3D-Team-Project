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

    public GameObject town;
    public GameObject ruin;
    public GameObject forest;
    public GameObject gorge;
    
    public MiniMap minimap;
    public Material skyboxMaterial;

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
        RenderSettings.skybox = skyboxMaterial;
        var spawnedPlayer = ObjectManager.Instance.SpawnPlayer(RespawnPos.position);
        SetPlayerReferences(spawnedPlayer);
        player = spawnedPlayer.GetComponent<Player>();

    }
    void Update()
    {
        Revive();
    }
    void Revive()
    {
        if (player.isDie && player.canRevive && Input.anyKeyDown)
        {
            town.SetActive(true);
            UIManager.Instance.GameOver.SetActive(false);
            var spawnedPlayer = ObjectManager.Instance.SpawnPlayer(RespawnPos.position);
            
            SetPlayerReferences(spawnedPlayer);
            RenderSettings.skybox = skyboxMaterial;
            UIManager.Instance.frontHpBar.fillAmount = 1;
            UIManager.Instance.frontSteminaBar.fillAmount = 1;
            player.Init();
            minimap.isTown = true;
            ruin.SetActive(false);
            forest.SetActive(false);
            gorge.SetActive(false);
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
