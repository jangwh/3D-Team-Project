using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapMonsterSpawner : MonoBehaviour //맵의 자식으로 빈오브젝트를 추가한 후 부착하세요
{
    [Header("이 맵에 스폰될 몬스터 배치 정보")]
    [Tooltip("Awake 시 자동으로 수집됩니다. ")]
    public List<MonsterPlacementInfo> monsterPlacements = new List<MonsterPlacementInfo>();

    [Header("몬스터 인스턴스 컨테이너 (선택 사항)")]
    [Tooltip("몬스터들이 특정 자식 오브젝트 아래에 있다면 여기에 할당. 없으면 이 스크립트의 자식들을 탐색.")]
    public Transform monsterInstancesContainer;

    [Header("맵의 순찰 지점 컨테이너")]
    [Tooltip("이 맵의 모든 순찰 지점 빈 오브젝트들을 담고 있는 부모 오브젝트를 할당해주세요.")]
    public Transform mapPatrolPointsContainer;

    void OnEnable()
    {
        ActivateMonsters();
    }

    void OnDisable()
    {
        DeactivateMonsters();
    }

    [ContextMenu("현재 맵에 있는 몬스터 정보 수집")]
    void CollectInitialMonsterPlacements() //인스펙터 메뉴창에서 선택해서 불러옵니다.
    {
        monsterPlacements.Clear(); //초기화

        Transform container = (monsterInstancesContainer != null) ? monsterInstancesContainer : transform;

        foreach (Transform child in container)
        {
            Monster monster = child.GetComponent<Monster>();
            if (monster != null)
            {
                GameObject prefabRef = monster.myPrefab;

                if (prefabRef != null)
                {
                    monsterPlacements.Add(new MonsterPlacementInfo
                    {
                        prefabToSpawn = prefabRef,
                        position = child.position,
                        rotation = child.rotation,
                        specificPatrolPoints = null //이 부분은 인스펙터에서 수동으로 할당해야 합니다!
                    });

                    //기존에 씬에 배치된 몬스터 인스턴스는 비활성화
                    child.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning($"Monster '{child.name}' 이 원본 프리펩 설정이 되지 않았습니다! 확인해주세요", child.gameObject);
                }
            }
        }
        Debug.Log($"'{gameObject.name}'에서 {monsterPlacements.Count}개의 몬스터 배치 정보를 수집했습니다. 순찰 정보를 수정해주세요!");
    }

    public void ActivateMonsters() //이 맵의 몬스터들을 스폰합니다!. (맵이 활성화될 때 Portal에서 불러옵니다.)
    {
        Debug.Log($"{gameObject.name}: 몬스터들을 활성화합니다.");

        for (int i = 0; i < monsterPlacements.Count; i++)
        {
            MonsterPlacementInfo placement = monsterPlacements[i];
            if (placement.prefabToSpawn != null)
            {
                GameObject spawnedMonsterGO = LeanPool.Spawn(
                    placement.prefabToSpawn,
                    placement.position,
                    placement.rotation
                );

                placement.spawnedInstance = spawnedMonsterGO;
                monsterPlacements[i] = placement;

                Monster monsterComponent = spawnedMonsterGO.GetComponent<Monster>();
                if (monsterComponent != null)
                {
                    monsterComponent.Initialize(Monster.MonsterIdleState.Patrol, placement.specificPatrolPoints); //각각의 몬스터 인스턴스에게 정보를 제공합니다.
                }
            }
        }
    }

    public void DeactivateMonsters()
    {
        Debug.Log($"{gameObject.name}: 몬스터들을 비활성화합니다.");
        for (int i = 0; i < monsterPlacements.Count; i++)
        {
            MonsterPlacementInfo placement = monsterPlacements[i];
            if (placement.spawnedInstance != null && placement.spawnedInstance.activeInHierarchy)
            {
                LeanPool.Despawn(placement.spawnedInstance);
                placement.spawnedInstance = null;
                monsterPlacements[i] = placement;
            }
        }
    }
}
