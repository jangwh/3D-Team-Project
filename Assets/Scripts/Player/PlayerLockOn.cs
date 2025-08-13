using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLockOn : MonoBehaviour
{
    public float lockOnRange = 20f;
    public LayerMask enemyLayer;

    public Transform playerTransform;
    public Transform CameraTarget;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject lockOnMarkerPrefab;

    private List<EnemyTarget> enemies = new List<EnemyTarget>();
    [HideInInspector]public EnemyTarget currentTarget;
    private int targetIndex = 0;
    private bool isLockedOn = false;
    [HideInInspector]public GameObject lockOnMarker;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isLockedOn)
                Unlock();
            else
                TryLockOn();
        }

        if (isLockedOn && currentTarget != null)
        {
            // 플레이어가 타겟을 바라봄
            Vector3 dir = currentTarget.transform.position - playerTransform.position;
            dir.y = 0;
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);

            // 타겟 전환
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SwitchTarget(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                SwitchTarget(1);

            // 락온 마커 위치 갱신
            if (lockOnMarker)
                lockOnMarker.transform.position = Camera.main.WorldToScreenPoint(currentTarget.lockOnPoint.position);

            if (currentTarget.monster.isDie)
            {
                Unlock();
            }
        }
    }

    void TryLockOn()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange, enemyLayer);
        enemies.Clear();

        foreach (var hit in hits)
        {
            EnemyTarget et = hit.GetComponent<EnemyTarget>();
            if (et != null)
                enemies.Add(et);
        }

        if (enemies.Count == 0) return;

        // 가장 가까운 타겟
        enemies.Sort((a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        targetIndex = 0;
        LockTo(enemies[targetIndex]);
    }

    void LockTo(EnemyTarget target)
    {
        currentTarget = target;
        isLockedOn = true;

        if (virtualCamera != null)
            virtualCamera.LookAt = target.lockOnPoint;

        // 락온 마커 생성
        if (lockOnMarkerPrefab != null)
            lockOnMarker = Instantiate(lockOnMarkerPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    void Unlock()
    {
        currentTarget = null;
        isLockedOn = false;

        if (virtualCamera != null)
            virtualCamera.LookAt = CameraTarget;

        if (lockOnMarker != null)
            Destroy(lockOnMarker);
    }

    void SwitchTarget(int direction)
    {
        if (enemies.Count < 2) return;

        targetIndex = (targetIndex + direction + enemies.Count) % enemies.Count;
        LockTo(enemies[targetIndex]);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lockOnRange);
    }
}