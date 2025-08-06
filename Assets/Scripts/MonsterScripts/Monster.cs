using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Character
{
    public enum MonsterState //몬스터의 기본 상태 2가지
    {
        Idle, Patrol
    }

    [Header("몬스터 설정")]
    public MonsterState initialState = MonsterState.Idle; // 초기 상태 (대기 또는 순찰)

    [Header("순찰 설정")]
    public Transform[] patrolPoints; // 순찰 지점들
    public float patrolWaitTime = 2f;

    [Header("플레이어 추적 설정")]
    public float detectionRadius = 10f; // 플레이어를 감지할 반경
    public float losePlayerDistance = 15f; //플레이어를 놓치는 거리 (감지 반경보다 커야함)
    public float detectionForwardOffset = 2f; // 감지 구체를 앞으로 이동시킬 거리
    public float chaseStateChangeWaitTime = 1f; //추적 상태 변경 시 대기 시간
    public LayerMask playerLayer;       // 플레이어의 레이어

    private Transform player;             // 플레이어의 Transform
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigid;
    private int currentPatrolIndex = 0;   // 현재 순찰 지점 인덱스
    private MonsterState currentState;    // 현재 몬스터의 행동 상태
    private bool isChasing = false;
    private bool isWaiting = false;

    void Awake()
    {
        currentState = initialState; //초기 상태로 설정
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        if (initialState == MonsterState.Patrol && (patrolPoints == null || patrolPoints.Length == 0))
        {
            print("몬스터가 순찰 상태로 설정되었지만 순찰 지점이 없습니다. 대기 상태로 전환합니다.");
            currentState = MonsterState.Idle;
        }

        rigid.isKinematic = true;

        navMeshAgent.speed = moveSpeed;
        navMeshAgent.angularSpeed = rotateSpeed;
    }

    void Update()
    {
        CheckForPlayer(); //지속적으로 추적상태 업데이트

        if (isChasing)
        {
            ChasePlayer(); 
        }
        else
        {
            PerformInitialStateBehavior();
        }
    }

    private void PerformInitialStateBehavior()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Patrol:
                Patrol();
                break;
        }
    }

    private void Idle()
    {
        if (navMeshAgent.hasPath)
        {
            navMeshAgent.ResetPath();
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0 || isWaiting) return;

        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(patrolWaitTime);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);

        isWaiting = false;
    }

    private void ChasePlayer() //navmesh로 찾기, rigidbody를 kinematic으로 두고 상황에 따라 풀기
    {
        //처음에 감지하고 나서는 플레이어가 어느정도 멀어져도 따라오도록 만들기. 일정 거리를 벗어나면 CheckForPlayer로 isChasing상태를 초기화
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    private void CheckForPlayer()
    {
        //1. 처음 플레이어를 Physics.OverlapSphere를 통해 감지하면 isChasing을 true로 만든다.
        //이 때 OverlapSphere의 위치는 몬스터의 살짝 앞쪽으로 해서 뒤쪽은 짧게, 앞은 길게 감지 하도록 함
        //2. 플레이가 일정 거리를 벗어나면 ChasePlayer() 함수에서 이 함수를 불러와서 상태 초기화

        if (isChasing)
        {
            if (player == null || Vector3.Distance(transform.position, player.position) > losePlayerDistance)
            {
                isChasing = false;
                player = null;
                print("플레이어를 놓쳤습니다. 순찰 상태로 복귀합니다.");
                StartCoroutine(StopAndResume(chaseStateChangeWaitTime));
            }
        }
        else
        {
            Vector3 detectionCenter = transform.position + transform.forward * detectionForwardOffset;
            Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius, playerLayer);
            if (hitColliders.Length > 0)
            {
                player = hitColliders[0].transform;
                isChasing = true;
                print("플레이어를 발견! 추적을 시작합니다.");
                StartCoroutine(StopAndResume(chaseStateChangeWaitTime));
            }
        }
    }

    private IEnumerator StopAndResume(float waitTime)
    {
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        navMeshAgent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 detectionCenter = transform.position + transform.forward * detectionForwardOffset;
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, losePlayerDistance);
    }
}
