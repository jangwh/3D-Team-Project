using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<ItemStatus> items = new List<ItemStatus>(); //�������� ������ ����Ʈ
    public static InventoryManager Instance { get; private set; }
    public static List<ItemStatus> Items => Instance.items;
    public List<ItemStatus> inventory = new List<ItemStatus>();

    public ItemSlot focusedSlot; //���콺�� �ö� ����
    public ItemSlotStore focusedSlotStore;
    public ItemSlot selectedSlot; //�巡�׸� ������ ����

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
    }
    void Start()
    {
        Refresh();
    }
    public static void PickupItem(ItemObject obj)
    {
        Instance.items.Add(obj.status);
        Destroy(obj.gameObject);
    }
    public static void Refresh()
    {
        UIManager.Instance.Inventory.Refresh(Instance.items);
    }
    public void AddItem(ItemStatus item)
    {
        if (item != null)
        {
            inventory.Add(item);
            Debug.Log(item.Data.name + "가 인벤토리에 추가되었습니다.");
        }
    }
    public static void Swap()
    {
        if (Instance.selectedSlot == null || Instance.focusedSlot == null) return;
        if (Instance.selectedSlot.Item == null || Instance.focusedSlot.Item == null) return;

        ItemStatus selectedItem = Instance.selectedSlot.Item;
        ItemStatus focusedItem = Instance.focusedSlot.Item;
        ItemStatus focusedItemStore = Instance.focusedSlotStore.Item;

        int selectedIndex = Items.IndexOf(selectedItem);
        int focusedIndex = Items.IndexOf(focusedItem);

        if (selectedIndex == -1 || focusedIndex == -1) return;

        ItemStatus temp = Items[selectedIndex];
        Items[selectedIndex] = Items[focusedIndex];
        Items[focusedIndex] = temp;

        Refresh();
        print($"{Instance.selectedSlot.Item.Data.itemName}�� {Instance.focusedSlot.Item.Data.itemName}��ġ�� �ٲ� ����");
    }
}
