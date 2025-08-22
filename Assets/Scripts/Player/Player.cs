using Lean.Pool;
using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Animator anim;
    public AudioSource audioSource;

    public float MaxStamina;
    public float currentStamina;
    public AudioClip[] audioClip;
    [HideInInspector]public bool isDie = false;
    [HideInInspector]public bool canRevive = false;
    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        canRevive = false;
    }
    IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(1f);
        LeanPool.Despawn(this);
        UIManager.Instance.GameOver.SetActive(true);
        canRevive = true;
    }
    public void OnFootSoundOne()
    {
        audioSource.PlayOneShot(audioClip[0]);
    }
    public void OnFootSoundTwo()
    {
        audioSource.PlayOneShot(audioClip[1]);
    }
    public void OnRunSoundOne()
    {
        audioSource.PlayOneShot(audioClip[2]);
    }
    public void OnRunSoundTwo()
    {
        audioSource.PlayOneShot(audioClip[3]);
    }
    public void OnJumpSound()
    {
        audioSource.PlayOneShot(audioClip[4]);
    }
    public void OnSwordSound()
    {
        audioSource.PlayOneShot(audioClip[5]);
    }
    public void OnAxeSound()
    {
        audioSource.PlayOneShot(audioClip[6]);
    }
    public void OnRollSound()
    {
        audioSource.PlayOneShot(audioClip[7]);
    }
    public void OnDrinkSound()
    {
        audioSource.PlayOneShot(audioClip[8]);
    }
    public void OnDie()
    {
        audioSource.PlayOneShot(audioClip[9]);
    }
}
