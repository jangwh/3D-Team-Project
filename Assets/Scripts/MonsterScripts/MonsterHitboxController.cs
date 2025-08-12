using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HitboxInfo
{
    public string hitboxName;
    public Collider hitboxCollider;
}

public class MonsterHitboxController : MonoBehaviour //히트박스를 관리하는 스크립트. Monster컴포넌트가 있는 오브젝트에 같이 붙입니다.
{
    public List<HitboxInfo> hitboxes; //모든 히트박스리스트

    void Start()
    {
        foreach (var info in hitboxes)
        {
            if (info.hitboxCollider != null)
            {
                info.hitboxCollider.gameObject.SetActive(false);
            }
        }
    }

    //각각의 스킬SO가 가지고 있는 히트박스 정보를 통해 히트박스를 끄고 킵니다.
    public void ActivateHitboxes(List<string> names, float duration, float damage)
    {
        List<Collider> collidersToActivate = new List<Collider>();

        foreach (string name in names)
        {
            HitboxInfo info = hitboxes.FirstOrDefault(h => h.hitboxName == name);
            if (info != null && info.hitboxCollider != null)
            {
                collidersToActivate.Add(info.hitboxCollider);
            }
            else
            {
                Debug.LogWarning($"'{name}'이라는 이름의 히트박스를 찾을 수 없습니다.");
            }
        }

        if (collidersToActivate.Count > 0)
        {
            StartCoroutine(HitboxSequence(collidersToActivate, duration, damage));
        }
    }

    private IEnumerator HitboxSequence(List<Collider> hitboxes, float duration, float damage)
    {
        foreach (var hitbox in hitboxes) //전부 활성화
        {
            if (hitbox == null) continue;

            DamageOnTrigger damageScript = hitbox.GetComponent<DamageOnTrigger>();
            if (damageScript != null)
            {
                damageScript.damage = damage;
                damageScript.ClearHittedList();
            }
            hitbox.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(duration); //대기 단계

        foreach (var hitbox in hitboxes) //전부 비활성화
        {
            if (hitbox == null) continue;
            hitbox.gameObject.SetActive(false);
        }

    }
}
