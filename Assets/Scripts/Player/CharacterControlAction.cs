using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator), typeof(RigBuilder))]
public class CharacterControllerAction : MonoBehaviour
{
    Animator anim;
    Rig rig;

    private WaitUntil untilReload;  //재장전 모션을 할때까지 대기하는 YieldIntruction
    private WaitForSeconds waitForReload; //재장전 모션을 수행하는동안 대기하는 YieldInstruction
    private bool isReloding; //재장전 할때 true

    void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<RigBuilder>().layers[0].rig; //RigBuilder에서 0번째 Rig를 가져옴
    }

    void Start()
    {
        untilReload = new WaitUntil(() => isReloding);
    }

    void Update()
    {
        if (!isReloding && Input.GetKeyDown(KeyCode.R))
        {
            //재장전
            rig.weight = 0;
            isReloding = true;
            anim.SetTrigger("Reload");
        }
    }
    public void ReloadEnd()
    {
        print("애니메이션 이벤트에 의해 호출된 재장전 끝");
        rig.weight = 1;
        //Animation rigging의 Rig기능은 애니메이션 클립의 Transform 변경값을 덮어 쓰므로
        //애니메이션 이벤트를 통해 weight를 조졸하는 기능을 제공하지 않는다. 원천적으로 차단되어있음.
        isReloding = false;
    }
}
