using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{ 
    public List<ItemStatus> items = new List<ItemStatus>(); 
    public List<ItemStatus> rememberedItems = new List<ItemStatus>();
    public List<ItemData> shoppingCartItems = new List<ItemData>();
    public static InventoryManager Instance { get; private set; }
    public static List<ItemStatus> Items => Instance.items;
    public static List<ItemStatus> RememberedItems => Instance.rememberedItems;

    public static List<ItemData> ShoppingCartItems =>
        Instance.shoppingCartItems;

    public ItemSlot focusedSlot; 
    public ItemSlot selectedSlot;
    
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Player player = FindObjectOfType<Player>();

            
            ItemStatus potion = Items.Find(i => i is ConsumableStatus);
            if (potion != null)
            {
                
                UIManager.Instance.Inventory.PortionUse(potion);
            }
            else
            {
                
                player.GetComponent<Animator>().SetTrigger("NoPotion");
            }
        }
    }
    public static void PickupItem(ItemObject obj)
    {
        Instance.items.Add(obj.status);
        Destroy(obj.gameObject);
    }

    public static void RemoveItem(ItemStatus obj)
    {
        Instance.items.Remove(obj);
        Refresh();
    }

    public static void RememberItem(ItemStatus obj)     //상점 슬롯에 넣었을 때
    {
        // Null check to prevent issues when called from DragMe or elsewhere
        if (obj == null)
        {
            Debug.LogWarning("Tried to remember a null item.");
            return;
        }

        // Remove from items if present
        if (Items.Contains(obj))
        {
            Items.Remove(obj);
        }
        // Only add to rememberedItems if not already present
        if (!RememberedItems.Contains(obj))
        {
            RememberedItems.Add(obj);
            Debug.Log($"{obj.Data.itemName}을 판매 슬롯에 배치했습니다.");
        }
        Refresh();
    }

    public static void ShoppingCart(DragMe2 obj)
    {
        if (obj.itemData != null)
        {
            Instance.shoppingCartItems.Add(obj.itemData);
        }
    }

    public static void SellRememberedItems()        //판매 확정
    {
        foreach (var item in Instance.rememberedItems)
        {
            Debug.Log($"{item.Data.itemName} 을 판매했습니다.");
        }
        Instance.rememberedItems.Clear();
        Refresh();
    }

    public static void BuyShoppingCart()
    {
        foreach (var data in Instance.shoppingCartItems)
        {
            if (data == null) continue; // 방어 코드

            ItemStatus newItem = new ItemStatus(data); // ItemData → ItemStatus 변환
            Debug.Log($"{data.itemName} 을 구매했습니다.");
            Instance.items.Add(newItem);
        }
        Instance.shoppingCartItems.Clear();
        Refresh();
    }

    public static void CancelSale()
    {
        foreach (var item in Instance.rememberedItems)
        {
            Items.Add(item);
            Debug.Log($"{item.Data.itemName} 을 다시 인벤토리에 돌려놨습니다.");
        }
        Instance.rememberedItems.Clear();
        Refresh();
    }
    
    public static void Refresh()
    {
        UIManager.Instance.Inventory.Refresh(Instance.items);

        ConsumableStatus potion = Items.Find(i => i is ConsumableStatus) as ConsumableStatus;
        if (potion != null)
        {
            UIManager.Instance.portionImage.sprite = potion.Data.icon;
        }
        else
        {
            UIManager.Instance.portionImage.sprite = null;
        }
    }
    public static void Swap()
    {
        if (Instance.selectedSlot == null || Instance.focusedSlot == null) return;
        if (Instance.selectedSlot.Item == null || Instance.focusedSlot.Item == null) return;

        ItemStatus selectedItem = Instance.selectedSlot.Item;
        ItemStatus focusedItem = Instance.focusedSlot.Item;

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
