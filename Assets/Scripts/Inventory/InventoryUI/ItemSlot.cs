using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Image icon;
    private ItemStatus item;
    public ItemStatus Item
    {
        get => item;
        set
        {
            item = value;
            icon.enabled = true;
            icon.sprite = value.Data.icon;
        }
    }
    void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
    }
    public void SetItem(ItemStatus item)
    {
        this.item = item;
        icon.enabled = true;
        icon.sprite = item.Data.icon;
    }

    public void Clear()
    {
        item = null;
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;
        UIManager.Inventory.ShowInfo(item);
    }

    #region 툴팁 표시
    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.focusedSlot = this;
        if (item == null) return;
        UIManager.Tooltip.Show(item, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.focusedSlot = null;
        if (item == null) return;
        UIManager.Tooltip.Hide();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (item == null) return;
        UIManager.Tooltip.Show(eventData.position);
    }
    #endregion
    public void OnBeginDrag(PointerEventData eventData)
    {
        icon.transform.SetParent(UIManager.Instance.transform); //아이콘 이미지를 Canvas 내에서 자유롭게 해줌
        InventoryManager.Instance.selectedSlot = this;
        UIManager.Tooltip.Hide();
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
        UIManager.Tooltip.Hide();
    }
}
