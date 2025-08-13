using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public ItemTooltip Tooltip;
    public InventoryPannel Inventory;

    public Image frontHpBar;
    public Image backHpBar;
    public Image frontSteminaBar;
    public Image backSteminaBar;
    public Image weaponImage;
    public Image portionImage;
    public RectTransform Canvas;

    public GameObject GameOver;
    public GameObject ItemGetAsk;
    public GameObject PressG;
    public GameObject ESCMenu;
    public GameObject ClearNotice;
    public MouseControl mouseControl;

    [HideInInspector] public float UImaxHp;
    [HideInInspector] public float UIcurrentHp;
    [HideInInspector] public float UImaxStamina;
    [HideInInspector] public float UIcurrentStamina;
    [HideInInspector] public bool isInventoryOn;
    [HideInInspector] public bool isESCMenuOn;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
    }

    void Start()
    {
        Inventory.gameObject.SetActive(false);
        Tooltip.Hide();
    }

    void Update()
    {
        InvetoryOnOff();
        ESCMenuOnOff();
        backHpBar.fillAmount = 1;
        backSteminaBar.fillAmount = 1;
    }
    void ESCMenuOnOff()
    {
        if(!isESCMenuOn)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ESCMenu.SetActive(true);
                isESCMenuOn = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ESCMenu.SetActive(false);
                isESCMenuOn = false;
            }
        }
    }
    void InvetoryOnOff()
    {
        if (!isInventoryOn)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Inventory.gameObject.SetActive(true);
                isInventoryOn = true;
                if (mouseControl != null)
                    mouseControl.ToggleUIMode(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
            {
                Inventory.gameObject.SetActive(false);
                isInventoryOn = false;
                if (mouseControl != null)
                    mouseControl.ToggleUIMode(false);
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