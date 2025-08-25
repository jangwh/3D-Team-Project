// DropMeStore.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DropMeStore : MonoBehaviour, IDropHandler
{
    public Image containerImage;
    public Image receivingImage;
    public Sprite defaultImage;
    public TextMeshProUGUI itemInfoText;
    public TextMeshProUGUI amountText;
    public Button buyButton;
    public Button sellButton;

    private Color normalColor;
    private ItemStatus storedItem;   // 슬롯에 저장된 아이템

    private void OnEnable()
    {
        if (containerImage != null)
            normalColor = containerImage.color;

        ClearSlot();
    }

    // 아이템 드롭
    public void OnDrop(PointerEventData eventData)
    {
        DragMe2 dragMe2 = eventData.pointerDrag.GetComponent<DragMe2>();
        if (dragMe2 != null && dragMe2.item != null)
        {
            storedItem = dragMe2.item;
            receivingImage.overrideSprite =
                dragMe2.GetComponent<Image>().sprite;
            InventoryManager.ShoppingCart(dragMe2);  
            sellButton.interactable = false;
        }
        
        DragMe dragMe = eventData.pointerDrag?.GetComponent<DragMe>();
        if (dragMe != null && dragMe.item != null)
        {
            // DropMeStore에 저장
            storedItem = dragMe.item;
            receivingImage.overrideSprite = dragMe.GetComponent<Image>().sprite;
            amountText.text = dragMe.item.amount.ToString();
            itemInfoText.text = dragMe.item.ToTooltipText();

            buyButton.interactable = false;

            // 인벤토리에서 제거 + 기억
            InventoryManager.RememberItem(dragMe.item);
            InventoryManager.RemoveItem(dragMe.item);
            
        }

        if (containerImage != null)
            containerImage.color = normalColor;
    }

    // 판매 버튼
    public void OnClickSell()
    {
        if (storedItem == null) return;

        InventoryManager.SellRememberedItems();
        ClearSlot();
        sellButton.interactable = true;
        buyButton.interactable = true;
    }

    // 취소 버튼
    public void OnClickCancel()
    {
        if (storedItem == null) return;

        InventoryManager.CancelSale();
        ClearSlot();
        sellButton.interactable = true;
        buyButton.interactable = true;
    }

    public void OnClickBuy()
    {
        InventoryManager.BuyShoppingCart();
        ClearSlot();
        sellButton.interactable = true;
        buyButton.interactable = true;
    }

    private void ClearSlot()
    {
        storedItem = null;
        if (receivingImage != null)
            receivingImage.overrideSprite = defaultImage;
        if (amountText != null)
            amountText.text = "0";
        if (itemInfoText != null)
            itemInfoText.text = string.Empty;
    }
}