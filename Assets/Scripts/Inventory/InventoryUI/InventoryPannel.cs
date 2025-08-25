using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ConsumableStatus;

public class InventoryPannel : MonoBehaviour
{
    private List<ItemSlot> slots = new List<ItemSlot>();
    public Transform info;
    public Image icon;
    public TextMeshProUGUI text;
    void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ItemSlot>());
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

        //for���� ����� ���
        //for (int i = 0; i < slots.Count; i++)
        //{
        //    if (i >= itemList.Count)
        //    {
        //        //�������� �����Ƿ� �κ��丮 ������ ���
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
        icon.sprite = itemStatus.Data.icon;
        text.text = itemStatus.ToTooltipText();
        if (!fullRefresh) return;
    }
    public void PortionUse(ItemStatus itemStatus)
    {
        Player player = GameManager.Instance.player;

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (itemStatus is ConsumableStatus)
            {
                (itemStatus as ConsumableStatus).Use(player);
                InventoryManager.Refresh();
                info.gameObject.SetActive(false);
            }
            else
            {
                // ������ ���� �� �ִϸ��̼�
                player.GetComponent<Animator>().SetTrigger("NoPotion");
            }
        }
    }
}
