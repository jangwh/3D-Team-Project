using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class WeaponSwapAndAttack : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string weaponName;
        public AnimatorController animatorController;
    }

    public List<WeaponData> weapons = new List<WeaponData>();

    [Header("References")]
    public CharacterControllerMove characterControllerMove; // 이동 제어 스크립트 참조
    private Animator anim;

    [Header("Combo Settings")]
    public List<string> comboAnimationNames = new List<string>() { "Attack1", "Attack2", "Attack3" };
    public float comboInputBufferTime = 0.5f;

    private int currentComboIndex = 0;
    private bool isAttacking = false;
    private bool bufferedInput = false;
    private float inputBufferTimer = 0f;

    private int currentWeaponIndex = 0;

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (weapons.Count == 0)
        {
            Debug.LogError("무기 데이터가 비어 있습니다!");
            return;
        }

        SetWeapon(currentWeaponIndex);
    }

    void Update()
    {
        HandleWeaponSwap();
        HandleAttackInput();
        UpdateInputBuffer();
    }

    void HandleWeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            SetWeapon(currentWeaponIndex);
        }
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                StartCombo();
            }
            else
            {
                bufferedInput = true;
                inputBufferTimer = comboInputBufferTime;
            }
        }
    }
    void StartCombo()
    {
        currentComboIndex = 0;
        PlayComboAnimation();
        isAttacking = true;
    }
    void PlayComboAnimation()
    {
        if (currentComboIndex < comboAnimationNames.Count)
        {
            string animName = comboAnimationNames[currentComboIndex];
            anim.Play(animName, 0, 0f);
        }
        else
        {
            ResetCombo();
        }
    }
    public void OnComboAnimationEnd() // 애니메이션 이벤트에서 호출됨
    {
        if (bufferedInput)
        {
            currentComboIndex++;
            bufferedInput = false;
            inputBufferTimer = 0f;
            PlayComboAnimation();
        }
        else
        {
            ResetCombo();
        }
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
    }
    void SetWeapon(int index)
    {
        var weapon = weapons[index];
        anim.runtimeAnimatorController = weapon.animatorController;
    }
}