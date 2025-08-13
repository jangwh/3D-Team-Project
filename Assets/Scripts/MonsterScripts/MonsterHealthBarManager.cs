using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;

public class MonsterHealthBarManager : MonoBehaviour
{
    public static MonsterHealthBarManager Instance { get; private set; }

    [Header("체력바 프리펩입니다.")]
    public GameObject healthBarPrefab;

    [Header("체력바가 표시될 부모 캔버스입니다.")]
    public Transform canvasTransform;

    private Dictionary<Monster, Slider> healthBars = new Dictionary<Monster, Slider>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }

    void LateUpdate()
    {
        foreach (var monster in healthBars.Keys)
        {
            //월드 좌표를 스크린 좌표로 변환
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(monster.uIPoint.position);
            //값 업데이트
            healthBars[monster].transform.position = screenPosition;
            healthBars[monster].value = monster.currentHp / monster.maxHp;
            //몬스터가 카메라 뒤에 있으면 체력 바를 숨김
            healthBars[monster].gameObject.SetActive(screenPosition.z > 0);
        }
    }

    public void ShowHealthBar(Monster monster)
    {
        if (monster.isBoss)
        {
            return;
        }

        if (!healthBars.ContainsKey(monster))
        {
            GameObject newhealthBar = LeanPool.Spawn(healthBarPrefab, canvasTransform);
            Slider slider = newhealthBar.GetComponent<Slider>();
            healthBars.Add(monster, slider); //현재 목록에 추가
        }
    }

    public void HideHealthBar(Monster monster)
    {
        if (monster.isBoss)
        {
            return;
        }

        if (healthBars.ContainsKey(monster))
        {
            LeanPool.Despawn(healthBars[monster].gameObject);
            healthBars.Remove(monster);
        }
    }
}
