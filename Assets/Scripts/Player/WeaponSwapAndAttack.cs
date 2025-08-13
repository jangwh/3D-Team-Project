using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwapAndAttack : MonoBehaviour
{
    public Player player;
    public List<WeaponData> weapons = new List<WeaponData>();

    [Header("References")]
    public CharacterControllerMove characterControllerMove; // 이동 제어 스크립트 참조
    public PlayerLockOn playerLockOn;
    public Collider GuardBox;
    public CapsuleCollider playerCollider;
    
    [Header("Sockets")]
    public Transform weaponSocket;
    
    private Animator anim;
    private GameObject currentWeaponGO;
    private Collider currentHitBox;


    public float comboInputBufferTime = 0.5f;

    private int currentComboIndex = 0;
    [HideInInspector]public bool isAttacking = false;
    private bool isGuard = false;
    private bool bufferedInput = false;
    private float inputBufferTimer = 0f;

    private int currentWeaponIndex = 0;


    void Awake()
    {
        anim = GetComponent<Animator>();
        GuardBox.enabled = false;
    }
    void Start()
    {
        anim.SetLayerWeight(1, 0);
        SetWeapon(currentWeaponIndex);
    }
    void Update()
    {
        if (player.isDie) return;
        if (player.currentStamina <= 0)
        {
            player.currentStamina = 0;
            isGuard = false;
            isAttacking = false;
            characterControllerMove.isJump = false;
            return;
        }
        HandleWeaponSwap();

        if (weapons[currentWeaponIndex].weaponPrefab != null)
        {
            HandleAttackInput();
            UpdateInputBuffer();
            Guard();
        }
    }
    void HandleWeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentWeaponGO != null) Destroy(currentWeaponGO);

            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            SetWeapon(currentWeaponIndex);

            StopAllCoroutines();
            ResetCombo();
            GuardBox.enabled = false;
        }
    }
    void Guard()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isGuard && !player.isDie && !characterControllerMove.isJump)
        {
            anim.SetLayerWeight(1, 1f);
            isGuard = true;
            anim.SetTrigger("Guard");
            UIManager.Instance.TakeStemina(3);
            player.currentStamina -= 3;
            
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            anim.SetLayerWeight(1, 0);
            isGuard = false;
        }
    }
    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && !isGuard && !player.isDie && characterControllerMove.isJump)
        {
            UIManager.Instance.TakeStemina(5);
            player.currentStamina -= 5;
            anim.SetTrigger("JumpAttack");
        }
        if (Input.GetMouseButtonDown(0) && !isGuard && !player.isDie && !characterControllerMove.isJump)
        {
            if (!isAttacking)
            {
                characterControllerMove.SetCanMove(false);
                StartAmberCombo();
            }
            else
            {
                bufferedInput = true;
                inputBufferTimer = comboInputBufferTime;
            }
        }
        if (Input.GetMouseButtonDown(1) && !isGuard && !player.isDie && !characterControllerMove.isJump)
        {
            if (!isAttacking)
            {
                characterControllerMove.SetCanMove(false);
                StartStrongCombo();
            }
            else
            {
                bufferedInput = true;
                inputBufferTimer = comboInputBufferTime;
            }
        }
    }
    void StartAmberCombo()
    {
        currentComboIndex = 0;
        PlayAmberComboAnimation();
        isAttacking = true;
    }
    void StartStrongCombo()
    {
        currentComboIndex = 0;
        PlayStrongComboAnimation();
        isAttacking = true;
    }
    void PlayAmberComboAnimation()
    {
        var comboList = weapons[currentWeaponIndex].amberAttackComboAnimationNames;
        if (currentComboIndex < comboList.Count)
        {
            anim.Play(comboList[currentComboIndex], 0, 0f);
            UIManager.Instance.TakeStemina(5);
            player.currentStamina -= 5;
        }
        else
        {
            ResetCombo();
        }
    }

    void PlayStrongComboAnimation()
    {
        var comboList = weapons[currentWeaponIndex].strongAttackComboAnimationNames;
        if (currentComboIndex < comboList.Count)
        {
            anim.Play(comboList[currentComboIndex], 0, 0f);
            UIManager.Instance.TakeStemina(10);
            player.currentStamina -= 10;
        }
        else
        {
            ResetCombo();
        }
    }

    // 애니메이션 이벤트에서 호출
    public void OnComboAnimationEnd()
    {
        var comboList = weapons[currentWeaponIndex].amberAttackComboAnimationNames;

        if (bufferedInput && currentComboIndex + 1 < comboList.Count)
        {
            currentComboIndex++;
            bufferedInput = false;
            inputBufferTimer = 0f;
            PlayAmberComboAnimation();
        }
        else
        {
            ResetCombo();
        }
    }

    // 애니메이션 이벤트에서 호출
    public void OnStrongComboAnimationEnd()
    {
        var comboList = weapons[currentWeaponIndex].strongAttackComboAnimationNames;

        if (bufferedInput && currentComboIndex + 1 < comboList.Count)
        {
            currentComboIndex++;
            bufferedInput = false;
            inputBufferTimer = 0f;
            PlayStrongComboAnimation();
        }
        else
        {
            ResetCombo();
        }
    }
    public void OnHitBoxActive()
    {
        StartCoroutine(HitOn());
    }
    public void OnGuardBoxActive()
    {
        StartCoroutine(GuardOn());
    }
    public void OnJumpAttackEnd()
    {
        anim.ResetTrigger("JumpAttack");
    }
    void UpdateInputBuffer()
    {
        if (bufferedInput)
        {
            inputBufferTimer -= Time.deltaTime;
            if (inputBufferTimer <= 0f)
            {
                bufferedInput = false;
            }
        }
    }
    void ResetCombo()
    {
        currentComboIndex = 0;
        isAttacking = false;
        bufferedInput = false;
        inputBufferTimer = 0f;
        characterControllerMove.SetCanMove(true);
    }
    void SetWeapon(int index)
    {
        var data = weapons[index];
        anim.runtimeAnimatorController = data.controllers;

        // 이전 무기 제거(안전)
        if (currentWeaponGO != null)
        {
            Destroy(currentWeaponGO);
            currentWeaponGO = null;
            currentHitBox = null;
        }

        // "맨손" 같은 슬롯 처리
        if (string.Equals(data.weaponName, "None", System.StringComparison.OrdinalIgnoreCase)
            || data.weaponPrefab == null)
        {
            characterControllerMove.isBattle = false;
            UIManager.Instance.weaponImage.sprite = data.weaponSprite;
            return;
        }

        characterControllerMove.isBattle = true;

        // 소켓에 인스턴스 생성
        currentWeaponGO = Instantiate(data.weaponPrefab, weaponSocket);
        currentWeaponGO.transform.localPosition = Vector3.zero;
        currentWeaponGO.transform.localRotation = Quaternion.identity;
        currentWeaponGO.transform.localScale = Vector3.one;

        // 히트박스는 인스턴스에서 찾기(자식 포함)
        currentHitBox = currentWeaponGO.GetComponentInChildren<Collider>(true);
        if (currentHitBox != null) currentHitBox.enabled = false;

        UIManager.Instance.weaponImage.sprite = data.weaponSprite;
    }
    void OnTriggerStay(Collider other)
    {
        if (playerLockOn != null &&
        playerLockOn.currentTarget != null &&
        playerLockOn.currentTarget.StunCollider != null &&
        playerLockOn.currentTarget.StunCollider.enabled)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(SpecialAttack());
                UIManager.Instance.TakeStemina(5);
                player.currentStamina -= 5;
                other.gameObject.GetComponentInParent<Character>().TakeDamage(150);
                playerLockOn.currentTarget.AfterSpecialAttack();
            }
        }
    }
    IEnumerator SpecialAttack()
    {
        playerCollider.enabled = false;
        anim.SetTrigger("SpecialAttack");
        print("특수공격");
        yield return new WaitForSeconds(1f);
        playerCollider.enabled = true;
    }
    IEnumerator HitOn()
    {
        if (currentHitBox != null)
        {
            currentHitBox.enabled = true;
            yield return new WaitForSeconds(0.4f);
            currentHitBox.enabled = false;
        }
    }
    IEnumerator GuardOn()
    {
        GuardBox.enabled = true;
        yield return new WaitForSeconds(1f);
        GuardBox.enabled = false;
    }
}