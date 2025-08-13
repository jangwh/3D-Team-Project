// DropMeStore.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DropMeStore : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image containerImage;
    public Image receivingImage;
    public Color highlightColor = Color.yellow;
    public Sprite defaultImage;
    public TextMeshProUGUI itemInfoText;
    public TextMeshProUGUI amountText;

    private Color normalColor;

    public DragMe lastMovedDragMe;
    public Sprite storedSprite;
    public int dragMeOriginalAmount; // 마지막 DragMe amount 저장
    public bool hasSavedState = false;

    public int storedAmount = 0;

    private void OnEnable()
    {
        if (containerImage != null)
            normalColor = containerImage.color;

        storedAmount = 0;
        UpdateAmountText(storedAmount);
        if (receivingImage != null)
            receivingImage.overrideSprite = defaultImage;
        storedSprite = defaultImage;
    }

    public void OnDrop(PointerEventData data)
    {
        var dragMe = data.pointerDrag?.GetComponent<DragMe>();
        if (dragMe == null) return;

        // Clear previous state
        lastMovedDragMe = null;
        hasSavedState = false;

        // Save new state
        lastMovedDragMe = dragMe;
        storedSprite = dragMe.GetComponent<Image>().sprite;
        dragMeOriginalAmount = dragMe.item.amount;
        hasSavedState = true;

        if (receivingImage != null)
            receivingImage.overrideSprite = storedSprite != null ? storedSprite : dragMe.defaultSprite;

        storedAmount = dragMe.item.amount;
        UpdateAmountText(storedAmount);

        dragMe.item.amount = 0;
        dragMe.UpdateAmountText();
        dragMe.SendMessage("UpdateSpriteIfEmpty", SendMessageOptions.DontRequireReceiver);

        ShowItemInfo(dragMe.item, storedAmount);

        if (containerImage != null)
            containerImage.color = normalColor;
    }

    public int ReceiveOneAmount(DragMe dragMe)
    {
        if (dragMe == null || dragMe.item == null || dragMe.item.amount <= 0)
            return 0;

        if (lastMovedDragMe != null && lastMovedDragMe != dragMe)
            PerformUndo();

        if (!hasSavedState)
        {
            lastMovedDragMe = dragMe;
            storedSprite = dragMe.GetComponent<Image>().sprite;
            dragMeOriginalAmount = dragMe.item.amount;
            hasSavedState = true;
        }

        dragMe.item.amount--;
        storedAmount++;

        dragMe.UpdateAmountText();

        if (receivingImage != null)
            receivingImage.overrideSprite = storedSprite != null ? storedSprite : dragMe.defaultSprite;

        UpdateAmountText(storedAmount);
        ShowItemInfo(dragMe.item, storedAmount);

        if (containerImage != null)
            containerImage.color = normalColor;

        return 1;
    }

    public void PerformUndo()
    {
        if (lastMovedDragMe == null || !hasSavedState) return;

        // DragMe 원래 상태 복원
        lastMovedDragMe.item.amount = dragMeOriginalAmount;
        var dragImage = lastMovedDragMe.GetComponent<Image>();
        if (dragImage != null)
            dragImage.sprite = storedSprite;
        lastMovedDragMe.UpdateAmountText();
        lastMovedDragMe.UpdateSpriteIfEmpty();

        // DropMeStore 초기화
        storedAmount = 0;
        if (receivingImage != null)
            receivingImage.overrideSprite = defaultImage;
        if (itemInfoText != null)
            itemInfoText.text = string.Empty;
        UpdateAmountText(storedAmount);

        lastMovedDragMe = null;
        hasSavedState = false;
        storedSprite = defaultImage;
    }

    public void ShowItemInfo(ItemStatus itemStatus, int totalAmount, bool fullRefresh = true)
    {
        if (itemStatus == null || itemStatus.Data == null) return;

        if (itemInfoText != null)
        {
            itemInfoText.text = itemStatus.ToTooltipText();
            itemInfoText.gameObject.SetActive(true);
        }

        if (amountText != null)
            amountText.text = totalAmount.ToString();
    }

    public void UpdateAmountText(int amount)
    {
        if (amountText != null)
            amountText.text = amount.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
        PerformUndo();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (containerImage == null) return;
        var dragMe = data.pointerDrag?.GetComponent<DragMe>();
        if (dragMe != null)
            containerImage.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (containerImage != null)
            containerImage.color = normalColor;
    }

    public void isSellButtonOn()
    {
        lastMovedDragMe = null;
        hasSavedState = false;
        storedAmount = 0;
        storedSprite = defaultImage;

        if (receivingImage != null)
            receivingImage.overrideSprite = defaultImage;
        if (itemInfoText != null)
            itemInfoText.text = string.Empty;
        if (amountText != null)
            amountText.text = "0";
    }
}