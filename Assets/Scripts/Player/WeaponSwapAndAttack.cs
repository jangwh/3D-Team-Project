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
    public int maxComboCount = 3;
    public float attackDelay = 0.5f;

    private int currentWeaponIndex = 0;
    private int currentComboIndex = 0;

    private Coroutine attackCoroutine;

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
            if (attackCoroutine == null)
            {
                currentComboIndex = 1;
                attackCoroutine = StartCoroutine(PerformCombo());
            }
            else if (currentComboIndex <= maxComboCount)
            {
                currentComboIndex++;
            }
        }
    }

    void SetWeapon(int index)
    {
        var weapon = weapons[index];
        anim.runtimeAnimatorController = weapon.animatorController;
    }

    IEnumerator PerformCombo()
    {
        characterControllerMove?.SetCanMove(false); // 이동 제한 시작

        while (currentComboIndex > 0)
        {
            anim.SetTrigger("meleeAttack");
            anim.SetInteger("attackCount", currentComboIndex);

            yield return new WaitForSeconds(attackDelay);
            currentComboIndex--;
        }

        anim.SetTrigger("meleeAttackEnd");

        characterControllerMove?.SetCanMove(true); // 이동 제한 해제
        attackCoroutine = null;
    }
}