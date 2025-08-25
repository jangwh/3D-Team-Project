using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image icon;
    private ItemStatus item;
    public int itemNumber;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;
        UIManager.Instance.Inventory.ShowInfo(item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.focusedSlot = this;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this == null || this.Item == null)
        {
            eventData.pointerDrag = null;
            return;
        }

        if (this != null && this.itemNumber >= 0 && this.itemNumber < InventoryManager.Items.Count)
        {
            item = InventoryManager.Items[this.itemNumber];
        }
        else
        {
            Debug.LogWarning($"Invalid slot index: {this?.itemNumber}");
            item = null;
            eventData.pointerDrag = null;
            return;
        }

        icon.transform.SetParent(UIManager.Instance.transform);
        InventoryManager.Instance.selectedSlot = this;
        UIManager.Instance.Tooltip.Hide();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.transform.SetParent(transform);
        icon.transform.localPosition = Vector3.zero;
        InventoryManager.Swap();
        InventoryManager.Instance.selectedSlot = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        icon.rectTransform.anchoredPosition += eventData.delta;
        UIManager.Instance.Tooltip.Hide();
    }
}