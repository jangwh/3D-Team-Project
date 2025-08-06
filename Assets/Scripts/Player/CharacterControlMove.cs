using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponSwapAndAttack;

public class CharacterControllerMove : MonoBehaviour
{
    CharacterController charCtrl;
    Animator anim;
    public WeaponData weaponData;
    public float moveSpeed; //걸을때 최고속도 = 1
    public float sprintSpeed; //뛸때 최고속도 = 3
    public bool isBattle;

    private float currentSpeed; //걷던 뛰던 현재 움직여야 하는 속도
    private float rawSpeed; //걷고 있으면 1, 뛰고 있으면 2가 되는 입력속도

    private float gravityVelocity;

    private bool canMove = true;


    void Awake()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!canMove) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 InputValue = Vector3.ClampMagnitude(new Vector3(x, 0, z), 1);

        float sprintValue = Input.GetAxis("Fire3");

        rawSpeed = InputValue.magnitude + sprintValue;

        //currentSpeed = 걸을 때의 속도 + 뛸 때의 속도((뛸 때의 최고속도 - 걸을 때의 최고속도) * sprintValue)

        currentSpeed = (InputValue.magnitude * moveSpeed) + ((sprintSpeed - moveSpeed) * sprintValue);

        Vector3 moveDir = transform.TransformDirection(InputValue) * currentSpeed;

        if (charCtrl.isGrounded)
        {
            //땅에 닿아 있으므로 중력 영향을 초기화.
            gravityVelocity = 0;
        }
        else
        {
            //땅에 닿지 않았으므로 중력의 영향 만큼 바닥으로 떨어져야 함.(중력 가속도 누적)
            gravityVelocity += Time.deltaTime * Physics.gravity.y;
        }

        moveDir.y = gravityVelocity;

        charCtrl.Move(moveDir * Time.deltaTime);

        if(weaponData.weaponName == "Fist")
        {
            isBattle = false;
        }
        else
        {
            isBattle = true;
        }

        if (!isBattle)
        {
            anim.SetFloat("XDir", x);
            anim.SetFloat("YDir", z);
            anim.SetFloat("Speed", rawSpeed); //걸을 때는 0~1, 뛸 때는 1~2
        }
        else
        {
            anim.SetBool("isBattle", isBattle);
            anim.SetFloat("XDir", x);
            anim.SetFloat("YDir", z);
            anim.SetFloat("Speed", rawSpeed);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("RollDodge");
        }
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
