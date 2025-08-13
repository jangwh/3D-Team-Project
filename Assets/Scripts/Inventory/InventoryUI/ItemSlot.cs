using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    private ItemStatus item;
    public UnityEvent onClick;  // 클릭 이벤트

    public ItemStatus Item
    {
        get => item;
        set
        {
            item = value;
            icon.enabled = value != null;
            if (value != null)
                icon.sprite = value.Data.icon;
        }
    }

    public void Clear()
    {
        item = null;
        icon.enabled = false;
    }

    public void OnPointerClick()
    {
        onClick?.Invoke();
    }

    public void UpdateSlotUI()
    {
        if (item != null)
            icon.sprite = item.Data.icon;
        else
            icon.sprite = null;
    }
}

