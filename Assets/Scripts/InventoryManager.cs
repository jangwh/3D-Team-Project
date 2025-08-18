using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<ItemStatus> items = new List<ItemStatus>(); //보유중인 아이템 리스트
    public static InventoryManager Instance { get; private set; }
    public static List<ItemStatus> Items => Instance.items;

    public ItemSlot focusedSlot; //마우스가 올라간 슬롯
    public ItemSlot selectedSlot; //드래그를 시작한 슬롯

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

            // 첫 번째 포션 찾기
            ItemStatus potion = Items.Find(i => i is ConsumableStatus);
            if (potion != null)
            {
                // 포션 사용
                UIManager.Instance.Inventory.PortionUse(potion);
            }
            else
            {
                // 포션이 없을 때 애니메이션
                player.GetComponent<Animator>().SetTrigger("NoPotion");
            }
        }
    }
    public static void PickupItem(ItemObject obj)
    {
        Instance.items.Add(obj.status);
        Destroy(obj.gameObject);
    }
    public static void Refresh()
    {
        UIManager.Instance.Inventory.Refresh(Instance.items);

        // 포션 이미지 자동 갱신
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
        print($"{Instance.selectedSlot.Item.Data.itemName}과 {Instance.focusedSlot.Item.Data.itemName}위치를 바꿀 예정");
    }
}
