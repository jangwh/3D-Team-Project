using Lean.Pool;
using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Animator anim;

    public float MaxStamina;
    public float currentStamina;
    [HideInInspector]public bool isDie = false;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void Start()
    {
        currentStamina = MaxStamina;
        base.Start();
        UIManager.Instance.UImaxHp = maxHp; 
        UIManager.Instance.UImaxStamina = MaxStamina;             
        UIManager.Instance.UIcurrentHp = currentHp; 
        UIManager.Instance.UIcurrentStamina = currentStamina;
    }
    void Update()
    {
        HP();
        Die();
    }
    public void HP()
    {
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }
    public override void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
    public override void Die()
    {
        if (isDie) return;

        if (currentHp <= 0 && !isDie)
        {
            isDie = true;
            anim.SetTrigger("Die");
            if(GameManager.Instance.playerLockOn.lockOnMarker != null)
            {
                Destroy(GameManager.Instance.playerLockOn.lockOnMarker);
            }
            StartCoroutine(DieAnimation());
        }
    }
    public void Init()
    {
        currentHp = maxHp;
        currentStamina = MaxStamina;
        UIManager.Instance.frontHpBar.fillAmount = 1;
        UIManager.Instance.frontSteminaBar.fillAmount = 1;
        anim.SetLayerWeight(1, 0);
        isDie = false;
    }
    IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(1f);
        LeanPool.Despawn(this);
        UIManager.Instance.GameOver.SetActive(true);
    }
}
