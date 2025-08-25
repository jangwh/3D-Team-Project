using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackSO : AttackPatternSO
{
    [Header("원거리 전용 설정")]
    public GameObject projectilePrefab;
    public override void Execute(Monster monster, Transform target, Transform firePoint = null)
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("원거리 공격을 위한 Projectile Prefab 또는 Fire Point가 설정되지 않았습니다.");
            return;
        }

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            float finalDamage = monster.damage * damageMultiplier;
            projectile.SetTarget(target, finalDamage);
        }
        
    }
}
