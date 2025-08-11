using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Image frontHpBar;
    public Image backHpBar;

    public Image frontSteminaBar;
    public Image backSteminaBar; 


    float maxHp;
    float currentHp;

    float maxStamina;
    float currentStamina;

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
    void Update()
    {
        if (GameManager.Instance.player != null)
        {
            maxHp = GameManager.Instance.player.maxHp;
            maxStamina = GameManager.Instance.player.MaxStamina;

            currentHp = GameManager.Instance.player.currentHp;
            currentStamina = GameManager.Instance.player.currentStamina;
        }
        backHpBar.fillAmount = maxHp;
        backSteminaBar.fillAmount = maxStamina;
    }
    public void TakeStemina(float stamina)
    {
        currentStamina -= stamina;
        float newSteminaRatio = currentStamina / maxStamina;

        frontSteminaBar.fillAmount = newSteminaRatio;
    }
}
