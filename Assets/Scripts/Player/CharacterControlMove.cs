using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static WeaponSwapAndAttack;

public class CharacterControllerMove : MonoBehaviour
{
    CharacterController charCtrl;
    Animator anim;
    public Player player;

    public float moveSpeed; //걸을때 최고속도 = 1
    public float sprintSpeed; //뛸때 최고속도 = 3
    public bool isBattle;

    private bool canMove = true;
    private bool isRollDodge = false;
    private bool isDie = false;

    private float currentSpeed; //걷던 뛰던 현재 움직여야 하는 속도
    private float rawSpeed; //걷고 있으면 1, 뛰고 있으면 2가 되는 입력속도

    private float gravityVelocity;

    public float turnSpeed;
    public Transform canTarget;

    public float lookUpSpeed;

    public float canTargetMaxHeight; //canTarget이 올라갈 수 있는 최대 높이
    public float canTargetMinHeight; //canTarget이 내려갈 수 있는 최소 높이

    void Awake()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!canMove) return;
        MosueMove();
        PlayerMove();
        Stamina();
        if (player.currentStamina <= 0)
        {
            player.currentStamina = 0;
            isRollDodge = false;
            return;
        }

        PlayerRollDodge();
        Die();
    }
    void Stamina()
    {
        if (player.currentStamina < player.MaxStamina)
        {
            player.currentStamina += 10 * Time.deltaTime;
        }
        else if (player.currentStamina >= player.MaxStamina)
        {
            player.currentStamina = player.MaxStamina;
        }
    }
    void Die()
    {
        if(isDie) return;

        if(player.currentHp <=0 && !isDie)
        {
            isDie = true;
            anim.SetTrigger("Die");
        }
    }
    void PlayerRollDodge() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRollDodge)
        {
            isRollDodge = true;
            anim.SetTrigger("RollDodge");
            player.currentStamina -= 20f;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isRollDodge = false;
        }
    }
    void PlayerMove()
    {
        if (player.currentStamina <= 0) return;
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
        if(currentSpeed >= 1)
        {
            player.currentStamina -= 0.1f * Time.deltaTime;
        }
    }
    void MosueMove()
    {
        if (!MouseControl.isFocused) return;

        //InputManager를 통해서 마우스 이동값 가져오가(mouseDelta)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //마우스 좌우 움직임에 맞게 Rotate
        transform.Rotate(0, mouseX * turnSpeed * Time.deltaTime, 0);

        Vector3 canTargetPos = canTarget.localPosition;
        //Mathf.Clamp(값, 최소값, 최대값) : 값이 만약 최소값이나 최대값 사이의 값이면 그대로 반환, 아니면 최소값이나 최대값을 반환
        canTargetPos.y = Mathf.Clamp(canTargetPos.y + (mouseY * lookUpSpeed * Time.deltaTime), canTargetMinHeight, canTargetMaxHeight);
        canTarget.localPosition = canTargetPos;
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
