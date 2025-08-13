// DragMe.cs
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public ItemStatus item;   // 드래그되는 아이템 데이터
    public bool dragOnSurfaces = true;
    public DropMeStore targetDrop;
    public Sprite defaultSprite;
    [Tooltip("UI Text to show the item amount")]
    public TextMeshProUGUI amountText;

    [HideInInspector]
    public Sprite currentSprite; // 드래그 시작 시 현재 이미지 저장

    private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

    // 현재 amount를 텍스트에 표시
    public void UpdateAmountText()
    {
        if (amountText == null)
            return;
        if (item != null && item.amount > 0)
            amountText.text = item.amount.ToString();
        else
            amountText.text = "";
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 Undo 수행
        if (targetDrop != null)
            targetDrop.PerformUndo();

        // 현재 이미지 저장
        currentSprite = GetComponent<Image>().sprite;

        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null) return;

        m_DraggingIcons[eventData.pointerId] = new GameObject("icon");
        m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

        var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;

        image.sprite = currentSprite;
        image.SetNativeSize();

        m_DraggingPlanes[eventData.pointerId] = dragOnSurfaces ? transform as RectTransform : canvas.transform as RectTransform;

        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons.ContainsKey(eventData.pointerId))
            SetDraggedPosition(eventData);
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        var rt = m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons.ContainsKey(eventData.pointerId) && m_DraggingIcons[eventData.pointerId] != null)
            Destroy(m_DraggingIcons[eventData.pointerId]);

        m_DraggingIcons[eventData.pointerId] = null;
        UpdateSpriteIfEmpty();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (targetDrop != null)
            {
                targetDrop.ReceiveOneAmount(this);
                eventData.Use();
            }
        }
        UpdateSpriteIfEmpty();
    }

    // amount가 0이면 기본 이미지로, 텍스트 갱신
    public void UpdateSpriteIfEmpty()
    {
        if (item != null && item.amount <= 0)
            GetComponent<Image>().sprite = defaultSprite;
        UpdateAmountText();
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();
        if (comp != null) return comp;

        var t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}