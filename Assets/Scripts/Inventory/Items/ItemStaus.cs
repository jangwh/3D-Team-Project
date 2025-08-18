using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static WeaponSwapAndAttack;

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

    public void Use(Player player)
    {
        Animator anim = player.GetComponent<Animator>();

        foreach (ConsumeValue consumeValue in Data.consumes)
        {
            if (consumeValue.type == ConsumeType.Health)
            {
                float newHp = player.currentHp + consumeValue.value;
                player.currentHp = Mathf.Min(newHp, player.maxHp);

                // UI 갱신
                UIManager.Instance.UIcurrentHp = player.currentHp;
                float newHpRatio = player.currentHp / player.maxHp;
                UIManager.Instance.frontHpBar.fillAmount = newHpRatio;
            }
        }

        // 포션 사용 애니메이션
        if (anim != null)
        {
            anim.SetTrigger("UsePotion");
        }

        // 인벤토리에서 제거
        InventoryManager.Items.Remove(this);

        // UI 포션 이미지 갱신
        if (!InventoryManager.Items.Exists(i => i is ConsumableStatus))
        {
            UIManager.Instance.portionImage.sprite = null;
        }
    }
    [Serializable]
    public class WeaponStatus : ItemStatus
    {
        public new WeaponData Data => data as WeaponData;

        public WeaponStatus(WeaponData data) : base(data)
        {
            if (data is not WeaponData)
            {
                Debug.LogError("WeaponData가 아닙니다.");
                return;
            }
        }
    }
}
