using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapAndAttack : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string weaponName;
        public RuntimeAnimatorController controller; // Base or Override 가능
    }

    public List<WeaponData> weapons = new List<WeaponData>();
    private Animator anim;
    private AnimatorOverrideController workingOverride;

    private int currentWeaponIndex = 0;

    public int attackCount = 0;
    public int maxCount = 3;
    public float exitTime;

    public float overheat;

    public bool max_combo = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (weapons.Count == 0)
        {
            Debug.LogError("무기 데이터가 비어 있습니다!");
            return;
        }

        SetWeaponController(currentWeaponIndex);
    }

    void Update()
    {
        // 무기 교체 (Tab)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            SetWeaponController(currentWeaponIndex);
        }

        if (Input.GetMouseButtonDown(0) && attackCount < maxCount && !max_combo)
        {
            attackCount++;
            if (attackCount == maxCount)
            {
                max_combo = true;
            }

            if (attackCount == 1 && !max_combo)
            {
                StartCoroutine(Timer());
            }
        }
    }

    void SetWeaponController(int index)
    {
        var weapon = weapons[index];

        // 항상 새 오버라이드 컨트롤러 생성 (base 또는 override 모두 지원)
        workingOverride = new AnimatorOverrideController(weapon.controller);
        anim.runtimeAnimatorController = workingOverride;
    }
    private IEnumerator Timer()
    {
        int loopCount = 0;

        while (attackCount > 0) // 0보다낮으면 종료
        {
            loopCount++;
            if (loopCount == 4)
            {
                Debug.Log("오버플로우 감지");

                // 복구작업 및 코루틴 탈출
                loopCount = 0;
                attackCount = 0;
                anim.SetInteger("attackCount", loopCount);
                break;
            }
            anim.SetTrigger("meleeAttack");
            anim.SetInteger("attackCount", loopCount);

            yield return new WaitForSeconds(exitTime); // 공격애니메이션 출력시간겸 대기
            attackCount--; // 카운터 내리기 
        }

        // 종료지점
        // 공격 카운트가 0이 되면 최대 콤보 상태 해제
        anim.SetTrigger("meleeAttackEnd");

        if (max_combo)
        {
            Debug.Log("과열");
            Invoke("overheatDelay", overheat);
        }
    }
    void overheatDelay()//과도한 공격방지
    {
        max_combo = false;
    }
}