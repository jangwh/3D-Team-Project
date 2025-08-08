using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponSwapAndAttack;

public class Player : Character
{
    public float MaxStamina;
    public float currentStamina;
    protected override void Start()
    {
        currentStamina = MaxStamina;
        base.Start();
    }
    public override void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
}
