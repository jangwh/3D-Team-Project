using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class ItemObject : MonoBehaviour
{
    public ItemData data;
    internal ItemStatus status;

    private float playerDistance;

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
    void Update()
    {
        Distance();
    }
    void Distance()
    {
        playerDistance = (transform.position - GameManager.Instance.player.transform.position).magnitude;
        if (playerDistance < 1.5f)
        {
            UIManager.Instance.ItemGetAsk.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Z))
            {
                InventoryManager.PickupItem(this);
                UIManager.Instance.ItemGetAsk.SetActive(false);
                InventoryManager.Refresh();
            }
        }
        else
        {
            UIManager.Instance.ItemGetAsk.SetActive(false);
        }
    }
}
