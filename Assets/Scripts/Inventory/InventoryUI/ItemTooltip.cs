using System.Collections;
using System.Collections.Generic;
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
    }
    public void Show(ItemStatus status, Vector2 pos)
    {
        if (!gameObject.activeSelf)
        {
            //현재 비활성화 상태라면
            gameObject.SetActive(true);
        }
        Show(pos);
        //text에 status를 텍스트로 출력
        text.text = status.ToTooltipText();
    }
    public void Show(Vector2 pos)
    {
        rect.anchoredPosition = pos;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
