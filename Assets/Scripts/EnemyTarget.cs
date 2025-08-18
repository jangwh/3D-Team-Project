using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTarget : MonoBehaviour //Monster.cs가 달려있는 오브젝트에 같이 달려 있습니다.
{
    public Transform lockOnPoint;
    public Collider StunCollider;
    public Monster monster;
    public float stunDuration = 3f;
    public float afterStunDuration = 3f;
    public float stunCooldown = 20f;
    public Animator animator;
    private NavMeshAgent monsterNavMeshAgent;

    [SerializeField] private float currentCooldown = 0f;

    void Awake()
    {
        monster = GetComponentInParent<Monster>();
        animator = GetComponentInParent<Animator>();
        monsterNavMeshAgent = GetComponentInParent<NavMeshAgent>();
    }
    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (currentCooldown < 0)
        {
            currentCooldown = 0;
        }

        if (monster.currentHp / monster.maxHp <= 0.5f && !monster.isStun && currentCooldown <= 0 && !monster.isDie)
        {
            Stun();
        }
    }
    void Start()
    {
        StunCollider.enabled = false;
    }

    public void Stun()
    {
        if (monster.isStun) return;

        animator.SetBool("Stun", true);
        monster.isStun = true;
        StunCollider.enabled = true;
        StartCoroutine(StunCoroutine());
    }

    public void AfterSpecialAttack()
    {
        StopAllCoroutines();

        StartCoroutine(AfterSpecialAttackCoroutine());
    }

    public void EndStun()
    {
        if (!monster.isStun) return;

        StopCoroutine(StunCoroutine());

        monster.SetIsAttacking(false);
        if (monsterNavMeshAgent.isOnNavMesh)
        {
            monsterNavMeshAgent.isStopped = false;
        }
        animator.SetBool("Stun", false);
        monster.isStun = false;
        currentCooldown = stunCooldown;
    }

    private IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(stunDuration);
        StunCollider.enabled = false;
        EndStun();
    }

    private IEnumerator AfterSpecialAttackCoroutine()
    {
        StunCollider.enabled = false;
        yield return new WaitForSeconds(afterStunDuration);
        EndStun();
    }

    void OnDrawGizmos()
    {
        if (lockOnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lockOnPoint.position, 0.2f);
        }
    }
}