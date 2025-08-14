using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPannel : MonoBehaviour
{
    private List<ItemSlot> slots = new List<ItemSlot>();
    private Transform info;

    void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ItemSlot>());
        info = transform.Find("Info");
    }
    void Start()
    {
        info.gameObject.SetActive(false);
    }
    public void Refresh(List<ItemStatus> itemList)
    {
        IEnumerator<ItemStatus> itemEnum = itemList.GetEnumerator();
        foreach (ItemSlot slot in slots)
        {
            if (itemEnum.MoveNext())
            {
                slot.Item = itemEnum.Current;
            }
            else
            {
                slot.Clear();
            }
        }
        return;

        //for문을 사용한 방법
        //for (int i = 0; i < slots.Count; i++)
        //{
        //    if (i >= itemList.Count)
        //    {
        //        //아이템이 없으므로 인벤토리 슬롯을 비움
        //        slots[i].Clear();
        //    }
        //    else
        //    {
        //        slots[i].Item = itemEnum.Current;
        //    }
        //}
    }
    public void ShowInfo(ItemStatus itemStatus, bool fullRefresh = true)
    {
        info.gameObject.SetActive(true);
        info.Find("IconFrame/Icon").GetComponent<Image>().sprite = itemStatus.Data.icon;
        info.Find("InfoText").GetComponent<TextMeshProUGUI>().text = itemStatus.ToTooltipText();
        if (!fullRefresh) return;
    }
}
