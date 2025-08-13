using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class ItemObject : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler, IPointerDownHandler
{
    public ItemData data;
    internal ItemStatus status;

    public void OnPointerDown(PointerEventData eventData)
    {
        InventoryManager.PickupItem(this);
        UIManager.Tooltip.Hide();
        InventoryManager.Refresh();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Tooltip.Show(status, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Tooltip.Hide();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        UIManager.Tooltip.Show(eventData.position);
    }

    void Start()
    {
        if (status == null)
        {
            if (data is ConsumableData)
            {
                status = new ConsumableStatus(data as ConsumableData);
            }
            else
            {
                status = new ItemStatus(data);
            }
        }
    }
}
