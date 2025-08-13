using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour //보스 UI에 부착하는 스크립트입니다.
{
    public static BossHealthBar Instance { get; private set; }

    private Slider bossHealthSLider;
    private Monster currentBoss;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        bossHealthSLider = GetComponent<Slider>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentBoss != null)
        {
            bossHealthSLider.value = currentBoss.currentHp / currentBoss.maxHp;
        }
    }

    public void ShowBossUI(Monster monster)
    {
        currentBoss = monster;
        gameObject.SetActive(true);
    }

    public void HideBossUI()
    {
        gameObject.SetActive(false);
        currentBoss = null;
    }
}
