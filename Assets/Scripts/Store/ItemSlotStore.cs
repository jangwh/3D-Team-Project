using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotStore : MonoBehaviour
{
    public Image icon;
    private ItemStatus item;
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

    public System.Action OnClickAction; // 클릭 이벤트 외부 연결용

    public void Clear()
    {
        item = null;
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        // 툴팁 표시
        UIManager.Instance.Inventory.ShowInfo(item);

        // 상점 슬롯 클릭 시 액션 호출
        OnClickAction?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.focusedSlotStore = this;
        if (item == null) return;
        UIManager.Instance.Tooltip.Show(item, eventData.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (item == null) return;
        UIManager.Instance.Tooltip.UpdatePosition(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.focusedSlot = null;
        UIManager.Instance.Tooltip.Hide();
    }
}