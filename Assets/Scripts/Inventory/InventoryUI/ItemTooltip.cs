using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    TextMeshProUGUI text;
    RectTransform rect;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        rect = transform as RectTransform;
        gameObject.SetActive(false);
    }

    public void Show(ItemStatus status, Vector2 pos)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        text.text = status.ToTooltipText();
        rect.anchoredPosition = pos;
    }

    public void UpdatePosition(Vector2 pos)
    {
        if (gameObject.activeSelf)
            rect.anchoredPosition = pos;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}