using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwapAndAttack : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string weaponName;
        public RuntimeAnimatorController controllers;
        public GameObject weapon;
        public Collider HitBox;
        [Header("Combo Settings")]
        public List<string> amberAttackComboAnimationNames = new List<string>() { };
        public List<string> strongAttackComboAnimationNames = new List<string>() { };
    }

    public Player player;
    public List<WeaponData> weapons = new List<WeaponData>();

    [Header("References")]
    public CharacterControllerMove characterControllerMove; // 이동 제어 스크립트 참조
    public PlayerLockOn playerLockOn;
    private Animator anim;

    public float comboInputBufferTime = 0.5f;

    public Collider GuardBox;

    private int currentComboIndex = 0;
    [HideInInspector]public bool isAttacking = false;
    private bool isGuard = false;
    private bool bufferedInput = false;
    private float inputBufferTimer = 0f;

    private int currentWeaponIndex = 0;

    public CapsuleCollider playerCollider;

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (weapons.Count == 0)
        {
            Debug.LogError("무기 데이터가 비어 있습니다!");
            return;
        }
        SetWeapon(currentWeaponIndex);
        weapons[currentWeaponIndex].HitBox.enabled = false;
        GuardBox.enabled = false;
    }
    void Start()
    {
        anim.SetLayerWeight(1, 0);
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

        if (weapons[currentWeaponIndex].weapon != null)
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
            // 현재 무기 비활성화
            if (weapons[currentWeaponIndex].weapon != null)
            {
                weapons[currentWeaponIndex].weapon.SetActive(false);
            }
            // 스왑
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            SetWeapon(currentWeaponIndex);
            // 상태 초기화
            StopAllCoroutines(); // 이전 무기 히트박스/가드박스 코루틴 중지
            ResetCombo();        // 콤보 상태 및 이동 가능 상태 복원
            if (weapons[currentWeaponIndex].weapon != null)
            {
                weapons[currentWeaponIndex].HitBox.enabled = false;
                GuardBox.enabled = false;
            }
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
        var weapon = weapons[index];
        anim.runtimeAnimatorController = weapon.controllers;

        if (weapons[index].weaponName != "None")
        {
            anim.SetLayerWeight(1, 0);
            characterControllerMove.isBattle = true;
            weapons[index].weapon.SetActive(true);
        }
        else
        {
            characterControllerMove.isBattle = false;
        }
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
                other.gameObject.GetComponent<EnemyTarget>().TakeDamage(150);
                playerLockOn.currentTarget.isStun = false;
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
        weapons[currentWeaponIndex].HitBox.enabled = true;
        yield return new WaitForSeconds(0.4f);
        weapons[currentWeaponIndex].HitBox.enabled = false;
    }
    IEnumerator GuardOn()
    {
        GuardBox.enabled = true;
        yield return new WaitForSeconds(1f);
        GuardBox.enabled = false;
    }
}