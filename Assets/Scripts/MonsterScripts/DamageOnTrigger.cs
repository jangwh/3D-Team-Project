using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour //이 스크립트는 각각의 히트박스 오브젝트에 붙입니다.
{
    public float damage;

    private HashSet<Character> hittedTargets = new HashSet<Character>();

    void OnTriggerEnter(Collider other)
    {
        Character target = other.GetComponent<Character>();
        if (target != null && !hittedTargets.Contains(target))
        {
            Debug.Log($"{target.name}이(가) {gameObject.name} 공격에 맞음! 데미지: {damage}");
            UIManager.Instance.TakeHp(damage);
            target.TakeDamage(damage);
            hittedTargets.Add(target);
        }
    }

    public void ClearHittedList() //이전 공격에서 맞았던 대상 기록을 깨끗하게 지웁니다.
    {
        hittedTargets.Clear(); 
    }
}
