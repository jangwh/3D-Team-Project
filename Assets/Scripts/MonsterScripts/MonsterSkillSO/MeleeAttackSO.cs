using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "T02/Attack Patterns/Melee Attack")]
public class MeleeAttackSO : AttackPatternSO //단순하게 진입하면 쿨타임마다 데미지를 주는 방식입니다.
{
    public override void Execute(Monster monster, Transform target, Transform firePoint = null)
    {
        //공격 범위 내의 플레이어에게 데미지
        if (Vector3.Distance(monster.transform.position, target.position) <= attackRange)
        {
            Debug.Log("공격 범위 진입 성공!"); 
            Character character = target.GetComponent<Character>();
            if (character != null)
            {
                Debug.Log($"[MeleeAttackSO] {target.name}의 Character 컴포넌트 찾음. TakeDamage 호출 시도.전달할 데미지: {damage}");
                character.TakeDamage(damage);
                Debug.Log(target.name + "에게 근접 공격! 데미지: " + damage);
            }
            else
            {
                Debug.LogWarning($"[MeleeAttackSO] {target.name}에서 Character 컴포넌트를 찾지 못했습니다!");
            }
        }
    }
}
