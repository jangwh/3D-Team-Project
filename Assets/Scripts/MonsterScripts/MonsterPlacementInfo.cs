using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonsterPlacementInfo
{
    public GameObject prefabToSpawn;
    public Vector3 position;
    public Quaternion rotation;

    [Tooltip("이 몬스터가 사용할 순찰 지점 배열을 여기에 할당해주세요.")]
    public Transform[] specificPatrolPoints;

    [HideInInspector] public GameObject spawnedInstance; // 스폰된 몬스터 인스턴스 참조 (디스폰 시 사용)
}
