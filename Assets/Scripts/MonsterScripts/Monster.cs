using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Character, IPoolable
{
    public enum MonsterIdleState //몬스터의 Idle 상태 2가지
    {
        Idle, Patrol
    }

    [Header("원본 프리펩")] //몬스터 스폰할 때 사용합니다.
    public GameObject myPrefab;

    [Header("몬스터 설정")]
    public MonsterIdleState initialState = MonsterIdleState.Idle; // 초기 상태 (대기 또는 순찰)

    [Header("공격 패턴")]
    public List<AttackPatternSO> attackPatterns; // 사용할 스킬 목록
    public Transform firePoint; // 원거리 공격 발사 위치

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
    private Animator animator;
    private int currentPatrolIndex = 0;   // 현재 순찰 지점 인덱스
    private MonsterIdleState currentState;    // 현재 몬스터의 행동 상태

    //현상태
    private bool isChasing = false;
    private bool isWaiting = false;
    private bool isAttacking = false;
    private float CurrentSpeed;
    private bool isDie = false;
    private Vector3 initialPosition;
    private bool isStun = false;

    private Dictionary<AttackPatternSO, float> attackCooldowns = new Dictionary<AttackPatternSO, float>();
    private int currentAttackIndex = 0;
    private int dropran;

    void Awake()
    {
        currentState = initialState; //초기 상태로 설정
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;

        foreach (var pattern in attackPatterns)
        {
            attackCooldowns[pattern] = -999f;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (initialState == MonsterIdleState.Patrol && (patrolPoints == null || patrolPoints.Length == 0))
        {
            print("몬스터가 순찰 상태로 설정되었지만 순찰 지점이 없습니다. 대기 상태로 전환합니다.");
            currentState = MonsterIdleState.Idle;
        }

        rigid.isKinematic = false;
        rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = true;

        navMeshAgent.speed = moveSpeed;
        navMeshAgent.angularSpeed = rotateSpeed;
    }

    void Update()
    {
        if (!isDie && !isStun) //죽은 상태나 기절 상태가 아니면, 움직인다.
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.nextPosition = transform.position;
            }

            CheckForPlayer(); //지속적으로 추적상태 업데이트

            if (isChasing)
            {
                ChaseAndAttackPlayer();
            }
            else
            {
                PerformInitialStateBehavior();
            }

            if (navMeshAgent.isOnNavMesh && !isAttacking)
            {
                rigid.velocity = navMeshAgent.velocity;
            }

            //애니메이션 업데이트 로직
            CurrentSpeed = rigid.velocity.magnitude;
            animator.SetFloat("Speed", CurrentSpeed);

            if (currentHp <= 0)
            {
                Die();
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        isDie = true;
        animator.SetTrigger("Die"); //애니메이션 이벤트로 SpawnItem()메서드를 불러온다.
        SpawnItem();
    }

    public void DespawnEvent() //Die애니메이션이 끝나면 에니메이션 이벤트에서 불러옵니다.
    {
        SpawnItem();
        LeanPool.Despawn(gameObject); //TODO : ObjectManager.cs에 몬스터 등록하는 로직 필요.
    }

    public void SpawnItem()
    {
        dropran = Random.Range(0, 100);
        if (dropran < 50)
        {
            Instantiate(DataManager.Instance.itemDatas[0].modelPrefab, transform.position, transform.rotation);
        }
    }

    public void OnSpawn() //Leanpool spawn으로 소환시 상태를 초기화 하는 메서드입니다.
    {
        //체력 리필 
        currentHp = maxHp;

        //초기 위치로 초기화
        transform.position = initialPosition;
        navMeshAgent.Warp(initialPosition);

        //상태 초기화
        isDie = false;
        isChasing = false;
        isAttacking = false;
        isWaiting = false;

        //NavMeshAgent 초기화
        navMeshAgent.isStopped = false;
        navMeshAgent.ResetPath();

        //애니메이션 초기화
        animator.Rebind();
        animator.Update(0f);

        //공격 쿨타임 초기화
        foreach(var pattern in attackPatterns)
        {
            attackCooldowns[pattern] = -999f;
        }
    }

    public void Initialize(MonsterIdleState initialMode, Transform[] specificPatrolPoints)
    {
        currentState = initialMode;
        patrolPoints = specificPatrolPoints;

        if (currentState == MonsterIdleState.Patrol && patrolPoints != null && patrolPoints.Length > 0)
        {
            navMeshAgent.SetDestination(patrolPoints[0].position);
        }
    }

    public void OnDespawn()
    {
        
    }

    public void SetIsAttacking(bool status) //콤보 공격시 사용예정
    {
        isAttacking = status;
    }

    private void ChaseAndAttackPlayer()
    {
        if (player == null || attackPatterns.Count == 0 || isAttacking) return; 
        //플레이어가 없거나, 공격패턴이 없거나, *공격 중* 이면 이동,회전 로직 중단!

        AttackPatternSO currentAttack = attackPatterns[currentAttackIndex];
        navMeshAgent.stoppingDistance = currentAttack.attackRange;
        navMeshAgent.SetDestination(player.position);

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }

            if (Time.time >= attackCooldowns[currentAttack] + currentAttack.attackCooldown) //쿨타임아니면 실행
            {
                float angle = Vector3.Angle(transform.forward, direction);

                if (angle < 5f)
                {
                    StartCoroutine(AttackSequence(currentAttack));
                }
            }
            else //쿨타임이면 다음 스킬로 넘어가기
            {
                currentAttackIndex = (currentAttackIndex + 1) % attackPatterns.Count;
            }
        }
    }

    private IEnumerator AttackSequence(AttackPatternSO currentAttack) //공격 루틴입니다.
    {
        isAttacking = true;
        navMeshAgent.isStopped = true; //행동 중단

        if (!string.IsNullOrEmpty(currentAttack.attackAnimationTrigger))
        {
            animator.SetTrigger(currentAttack.attackAnimationTrigger);
            print($"현재 실행된 trigger : {currentAttack.attackAnimationTrigger}");
        }
        
        attackCooldowns[currentAttack] = Time.time;
        
        yield return new WaitForSeconds(currentAttack.preAttackDelay); //선딜

        currentAttack.Execute(this, player); //공격. 히트박스 컨트롤러를 통해서 공격 유지시간을 설정합니다

    }

    //애니메이션 클립의 event에서 마지막 프레임에 event를 추가한 후 불러올 함수입니다. 
    //코루틴과 애니메이션의 길이를 일치시켜서 isAttacking의 토글에 문제가 없도록 만듭니다.
    public void OnAttackAnimationEnd()
    {
        currentAttackIndex = (currentAttackIndex + 1) % attackPatterns.Count;
        isAttacking = false;
        navMeshAgent.isStopped = false;
        Debug.Log("애니메이션 이벤트: 공격 종료!");

        ////쿨타임 디버그 라인입니다.
        //Debug.Log("---현재 스킬 쿨타임 현황---");
        //foreach (var pattern in attackPatterns)
        //{
        //    float lastAttackTime = attackCooldowns[pattern];
        //    float cooldownDuration = pattern.attackDuration;
        //    float remainingTime = (lastAttackTime + cooldownDuration) - Time.time;

        //    Debug.Log($"- {pattern.name}: {remainingTime:F2}초 남음"); 
        //}
        //Debug.Log("--------------------------");
    }

    private void PerformInitialStateBehavior()
    {
        switch (currentState)
        {
            case MonsterIdleState.Idle:
                Idle();
                break;
            case MonsterIdleState.Patrol:
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

        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 2f)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    private IEnumerator WaitAtPatrolPoint() //순찰 시 방향 전환할때 잠깐 멈추고 다음 이동 지점 설정
    {
        isWaiting = true;
        yield return new WaitForSeconds(patrolWaitTime);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);

        isWaiting = false;
    }

    private void CheckForPlayer()
    {
        //losePlayerDistance를 벗어나면 추적을 종료
        if (isChasing)
        {
            if (player == null || Vector3.Distance(transform.position, player.position) > losePlayerDistance)
            {
                isChasing = false;
                player = null;
                print("플레이어를 놓쳤습니다. 순찰 상태로 복귀합니다.");
                animator.SetInteger("IdleState", 0);

                navMeshAgent.ResetPath();

                StartCoroutine(StopAndResume(chaseStateChangeWaitTime));
                
            }
        }
        else
        {
            Vector3 detectionCenter = transform.position + transform.forward * detectionForwardOffset; 
            Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius, playerLayer);
            if (hitColliders.Length > 0) //콜라이더가 붙어있는 오브젝트를 찾습니다!
            {
                player = hitColliders[0].transform;
                isChasing = true;
                print("플레이어를 발견! 추적을 시작합니다.");
                animator.SetInteger("IdleState", 1);
                StartCoroutine(StopAndResume(chaseStateChangeWaitTime));
            }
        }
    }

    private IEnumerator StopAndResume(float waitTime) //순찰/공격 상태 변환시 바뀝니다.
    {
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        navMeshAgent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        //감지 범위 기즈모
        Gizmos.color = Color.red;
        Vector3 detectionCenter = transform.position + transform.forward * detectionForwardOffset;
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);

        //추적 제한 범위 기즈모
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, losePlayerDistance);

        //공격 범위 기즈모
        if (attackPatterns != null && attackPatterns.Count > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackPatterns[currentAttackIndex].attackRange);
        }
    }

    
}
