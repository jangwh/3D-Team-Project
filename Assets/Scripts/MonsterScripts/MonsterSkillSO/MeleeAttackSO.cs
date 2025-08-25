using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "T02/Attack Patterns/Melee Attack")]
public class MeleeAttackSO : AttackPatternSO 
{
    public override void Execute(Monster monster, Transform target, Transform firePoint = null)
    {
        float finalDamage = monster.damage * damageMultiplier;
        monster.GetComponent<MonsterHitboxController>()?.ActivateHitboxes(hitboxNames, attackDuration, finalDamage);
    }
}
