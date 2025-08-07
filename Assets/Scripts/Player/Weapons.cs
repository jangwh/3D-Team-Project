using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Player
{
    public float weapondamage = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyTarget enemy = other.GetComponent<EnemyTarget>();
            enemy.TakeDamage(weapondamage);
            print("데미지입힘");
        }
    }
}