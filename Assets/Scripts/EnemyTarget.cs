using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour //Monster.cs가 달려있는 오브젝트에 같이 달려 있습니다.
{
    public Transform lockOnPoint;
    public Collider StunCollider;
    public Monster monster;
    public float stunTime;

    void Awake()
    {
        monster = GetComponent<Monster>();
    }
    //void Update()
    //{
    //    if (monster.currentHp/monster.maxHp <= 0.5f)
    //    {
    //        Stun(stunTime);
    //    }
    //}
    void Start()
    {
        StunCollider.enabled = false;
    }

    public void Stun(float stunTime)
    {
        //animation Stun을 true로 만듬
        //스턴 콜라이더를 setactive true
        //코루틴으로 대기
        //스턴 콜라이더를 setactive false
        //animation Stun을 true로 만듬
    }

    private IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(stunTime);
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