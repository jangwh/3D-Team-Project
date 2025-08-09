using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Lean.Pool;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance { get; private set; }
    public Player playerPrefab;

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
        PrewarmPool(playerPrefab, 5);
    }
    void PrewarmPool<T>(T prefab, int count) where T : Component
    {
        for (int i = 0; i < count; i++)
        {
            var obj = LeanPool.Spawn(prefab, Vector3.zero, Quaternion.identity);
            obj.gameObject.SetActive(false);
            LeanPool.Despawn(obj);
        }
    }
    public Player SpawnPlayer(Vector3 position)
    {
        return LeanPool.Spawn(playerPrefab, position, Quaternion.identity);
    }
}
