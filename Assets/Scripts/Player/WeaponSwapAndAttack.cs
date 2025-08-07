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
        public GameObject weapon;
    }

    public List<WeaponData> weapons = new List<WeaponData>();

    [Header("References")]
    public CharacterControllerMove characterControllerMove; // 이동 제어 스크립트 참조
    private Animator anim;

    [Header("Combo Settings")]
    public List<string> amberAttackComboAnimationNames = new List<string>() { };
    public List<string> strongAttackComboAnimationNames = new List<string>() { };
    public List<string> guardComboAnimationNames = new List<string>() { };
    public float comboInputBufferTime = 0.5f;

    public Collider HitBox;


    private int currentComboIndex = 0;
    [HideInInspector]public bool isAttacking = false;
    private bool isGuard = false;
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
        HitBox.enabled = false;
    }

    void Update()
    {
        HandleWeaponSwap();
        HandleAttackInput();
        UpdateInputBuffer();
        Guard();
    }

    void HandleWeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            SetWeapon(currentWeaponIndex);
        }
    }
    void Guard()
    {
        if(Input.GetKeyDown(KeyCode.F) && !isGuard)
        {
            isGuard = true;
            string animName = guardComboAnimationNames[0];
            anim.Play(animName, 0, 0f);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            isGuard = false;
        }
    }
    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && !isGuard)
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
        if (Input.GetMouseButtonDown(1) && !isGuard)
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
        if (currentComboIndex < amberAttackComboAnimationNames.Count)
        {
            string animName = amberAttackComboAnimationNames[currentComboIndex];
            anim.Play(animName, 0, 0f);
        }
        else
        {
            ResetCombo();
        }
    }
    void PlayStrongComboAnimation()
    {
        if (currentComboIndex < strongAttackComboAnimationNames.Count)
        {
            string animName = strongAttackComboAnimationNames[currentComboIndex];
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
            PlayAmberComboAnimation();
        }
        else
        {
            ResetCombo();
        }
    }
    public void OnStrongComboAnimationEnd() // 애니메이션 이벤트에서 호출됨
    {
        if (bufferedInput)
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
        anim.runtimeAnimatorController = weapon.animatorController;
    }
    IEnumerator HitOn()
    {
        HitBox.enabled = true;
        yield return new WaitForSeconds(0.4f);
        HitBox.enabled = false;
    }
}