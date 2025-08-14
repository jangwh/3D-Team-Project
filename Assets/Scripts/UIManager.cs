using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public static ItemTooltip Tooltip { get; private set; }
    public static InventoryPannel Inventory { get; private set; }

    public Image frontHpBar;
    public Image backHpBar;

    public Image frontSteminaBar;
    public Image backSteminaBar;

    public Image weaponImage;
    public Image portionImage;

    public Transform Canvas;

    public GameObject GameOver;
    public GameObject ItemGetAsk;

    [HideInInspector]public float UImaxHp;
    [HideInInspector]public float UIcurrentHp;

    [HideInInspector]public float UImaxStamina;
    [HideInInspector]public float UIcurrentStamina;

    private bool isInventoryOn;

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
        Tooltip = Canvas.transform.GetComponentInChildren<ItemTooltip>();
        Inventory = Canvas.transform.GetComponentInChildren<InventoryPannel>();
    }
    void Start()
    {
        Tooltip.Hide();
        Inventory.gameObject.SetActive(false);
    }
    void Update()
    {
        InvetoryOnOff();
        backHpBar.fillAmount = 1;
        backSteminaBar.fillAmount = 1;
    }
    void InvetoryOnOff()
    {
        if(!isInventoryOn)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Inventory.gameObject.SetActive(true);
                isInventoryOn = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Inventory.gameObject.SetActive(false);
                isInventoryOn = false;
            }
        }
    }
    public void TakeHp(float hp)
    {
        UIcurrentHp -= hp;
        float newHpRatio = UIcurrentHp / UImaxHp;

        frontHpBar.fillAmount = newHpRatio;
    }
    public void TakeStemina(float stamina)
    {
        UIcurrentStamina -= stamina;
        float newSteminaRatio = UIcurrentStamina / UImaxStamina;

        frontSteminaBar.fillAmount = newSteminaRatio;
    }
}
