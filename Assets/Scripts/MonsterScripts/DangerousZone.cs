using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousZone : MonoBehaviour
{
    private Monster monster;
    public int skillIndex;
    public GameObject dangerousZone;

    void Awake()
    {
        monster = GetComponentInParent<Monster>();
        dangerousZone.gameObject.SetActive(false);
    }
    void Update()
    {
        if (monster.currentAttackIndex == skillIndex)
        {
            dangerousZone.gameObject.SetActive(true);
        }
        else
        {
            dangerousZone.gameObject.SetActive(false);
        }
    }
}
