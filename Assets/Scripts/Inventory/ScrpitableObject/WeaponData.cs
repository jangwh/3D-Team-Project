using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Scriptable Object/Items/WeaponData")]
public class WeaponData : ItemData
{
    public float damage;
    public string weaponName;
    public RuntimeAnimatorController controllers;
    public GameObject weaponPrefab;
    public Sprite weaponSprite;
    [Header("Combo Settings")]
    public List<string> amberAttackComboAnimationNames = new List<string>() { };
    public List<string> strongAttackComboAnimationNames = new List<string>() { };
    protected override void Reset()
    {
        itemType = ItemType.Equip;
        icon = null;
        modelPrefab = null;
        itemName = "새 무기";
        description = "무기 설명을 입력하세요 (공격력, 내구도 등은 안 적어도 됩니다)";
        price = 0;
        damage = 0;
    }
}
