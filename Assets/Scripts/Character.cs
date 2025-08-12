using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHp;
    public float currentHp;
    public float damage;
    public float moveSpeed;
    public float rotateSpeed;
    public float lastAttackTime;
    public float attackInterval;

    protected virtual void Start()
    {
        currentHp = maxHp;
    }
    void Update()
    {
        Die();
    }
    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
    public virtual void Die()
    {

    }
}
