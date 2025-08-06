using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject //몬스터 패턴의 기반이 될 스크립트입니다.
{
    [Header("공통 공격 설정")]
    public float damage = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    public abstract void Execute(Monster monster, Transform target, Transform firePoint = null);
    //각 공격 패턴의 실제로직을 구현할 추상 메서드입니다. 
    //attacker: 공격을 수행하는 몬스터
    //target: 공격 대상 (플레이어)
    //firePoint: 원거리 공격 시 발사 위치 (필요 시 사용)
}
