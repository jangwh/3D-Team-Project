using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponSwapAndAttack;

public class Player : Character
{
    Animator anim;

    public float MaxStamina;
    public float currentStamina;
    private bool isDie = false;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void Start()
    {
        currentStamina = MaxStamina;
        base.Start();
    }
    public override void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
    public override void Die()
    {
            if (isDie) return;

            if (currentHp <= 0 && !isDie)
            {
                isDie = true;
                anim.SetTrigger("Die");
            }
    }
}
