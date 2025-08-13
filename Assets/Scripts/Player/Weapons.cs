using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Player player;
    public float weapondamage = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Character enemy = other.GetComponentInParent<Character>();
            enemy.TakeDamage(weapondamage);
            print("데미지입힘");
        }
    }
}