using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//소비할 경우 효과에 대한 타입 정의
public enum ConsumeType
{
    Health,
}

[Serializable]

public class ConsumeValue
{
    public ConsumeType type;
    public int value;
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Items/ConsumableData")]
public class ConsumableData : ItemData
{
    public int stackSize; //아이템이 겹쳐질 개수
    public ConsumeValue[] consumes; //사용할 경우의 효과
    protected override void Reset()
    {
        itemType = ItemType.Consumable;
        icon = null;
        modelPrefab = null;
        itemName = "새 소비 아이템";
        description = "이곳에 설명을 입력하세요.";
        stackSize = 1;
        consumes = new ConsumeValue[0];
    }
}
