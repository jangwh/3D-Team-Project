using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ItemStatus
{
    protected ItemData data;
    public ItemData Data
    {
        get => data; set => data = value;
    }
    public int uid;
    public int amount; //아이템 개수

    public ItemStatus(ItemData data, int amount = 1)
    {
        this.data = data;
        uid = data.uid;
        this.amount = amount;
    }

    public override string ToString()
    {
        //이 객체는 string으로 캐스팅 될 때 자동으로 json으로 변환됨
        return JsonUtility.ToJson(this);
    }
}
[Serializable]
public class ConsumableStatus : ItemStatus
{
    public new ConsumableData Data => data as ConsumableData;

    public ConsumableStatus(ConsumableData data, int amount = 1) : base(data, amount)
    {
        if (amount > Data.stackSize)
        {
            Debug.LogError($"소비 아이템은 한 칸에 최대 수량보다 큰 수량으로 생성할 수 없습니다.\n 생성한 수량 : {amount}. 최대 수량 : {Data.stackSize}");
        }
    }
    public void Use()
    {
        foreach (ConsumeValue consumeValue in Data.consumes)
        {
            Debug.Log($"{consumeValue.type}을 {consumeValue.value}만큼 증가시켰습니다.");
        }
        InventoryManager.Items.Remove(this);
    }
}
