using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform rect;

    public void Show(ItemStatus status, Vector2 pos)
    {
        if (!gameObject.activeSelf)
        {
            //현재 비활성화 상태라면
            gameObject.SetActive(true);
        }
        rect.anchoredPosition = pos;
        //text에 status를 텍스트로 출력
        text.text = status.ToTooltipText();
    }
    public void UpdatePosition(Vector2 pos)
    {
        if (gameObject.activeSelf)
        {
            rect.anchoredPosition = pos;
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
