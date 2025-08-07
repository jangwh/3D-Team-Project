using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponSwapAndAttack;

public class Player : Character
{
    protected override void Start()
    {
        base.Start();
    }
    public override void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
}
