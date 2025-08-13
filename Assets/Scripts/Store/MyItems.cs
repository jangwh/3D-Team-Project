using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class MyItems : MonoBehaviour
{ 
    private List<ItemSlot> slots = new List<ItemSlot>();
    public Transform infoPanel;
    public TextMeshProUGUI infoText;
    private static List<ItemStatus> items = new List<ItemStatus>();

    void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ItemSlot>());

        // 각 슬롯 클릭 이벤트 등록
        foreach (var slot in slots)
        {
            slot.onClick.AddListener(() => ShowInfo(slot.Item));
        }
    }

    // 인벤토리 리스트를 UI 슬롯에 반영
    public void Refresh(List<ItemStatus> itemList)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < itemList.Count)
            {
                slots[i].Item = itemList[i];
                slots[i].UpdateSlotUI();
            }
            else
            {
                slots[i].Clear();
            }
        }

        infoPanel.gameObject.SetActive(false);
        infoText.text = "";
    }

    // 클릭 시 아이템 정보를 infoText에 표시
    public void ShowInfo(ItemStatus itemStatus, bool fullRefresh = true)
    {
        if (itemStatus == null)
        {
            infoText.text = "";
            infoPanel.gameObject.SetActive(false);
            return;
        }

        infoPanel.gameObject.SetActive(true);
        infoText.text = itemStatus.ToTooltipText();
    }
}