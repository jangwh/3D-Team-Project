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

    public float moveSpeed; //������ �ְ�ӵ� = 1
    public float sprintSpeed; //�۶� �ְ�ӵ� = 3
    public bool isBattle;
    public bool isJump;

    private bool canMove = true;
    private bool isRollDodge = false;

    private float currentSpeed; //�ȴ� �ٴ� ���� �������� �ϴ� �ӵ�
    private float rawSpeed; //�Ȱ� ������ 1, �ٰ� ������ 2�� �Ǵ� �Է¼ӵ�

    private float gravityVelocity;

    public float turnSpeed;
    public Transform canTarget;

    public float lookUpSpeed;

    public float canTargetMaxHeight; //canTarget�� �ö� �� �ִ� �ִ� ����
    public float canTargetMinHeight; //canTarget�� ������ �� �ִ� �ּ� ����
    private MouseControl mouseControl;

    void Awake()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        mouseControl = FindObjectOfType<MouseControl>();
    }
    void Update()
    {
        if (mouseControl.isStoreOn) return;
        if (player.isDie) return;
        if (!canMove) return;
        MouseMove();
        PlayerMove();
        Stamina();
        if (player.currentStamina <= 0)
        {
            player.currentStamina = 0;
            isRollDodge = false;
            isJump = false;
            return;
        }
        PlayerRollDodge();
        Jump();
    }
    void Stamina()
    {
        if (player.currentStamina < player.MaxStamina)
        {
            if (isBattle) 
            {
                UIManager.Instance.TakeStemina(-5 * Time.deltaTime);
                player.currentStamina += 5 * Time.deltaTime;
            }
            else
            {
                UIManager.Instance.TakeStemina(-15 * Time.deltaTime);
                player.currentStamina += 15 * Time.deltaTime;
            }
        }
        else if (player.currentStamina >= player.MaxStamina)
        {
            player.currentStamina = player.MaxStamina;
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

        //currentSpeed = ���� ���� �ӵ� + �� ���� �ӵ�((�� ���� �ְ�ӵ� - ���� ���� �ְ�ӵ�) * sprintValue)

        currentSpeed = (InputValue.magnitude * moveSpeed) + ((sprintSpeed - moveSpeed) * sprintValue);

        Vector3 moveDir = transform.TransformDirection(InputValue) * currentSpeed;

        if (charCtrl.isGrounded)
        {
            //���� ��� �����Ƿ� �߷� ������ �ʱ�ȭ.
            gravityVelocity = 0;
        }
        else
        {
            //���� ���� �ʾ����Ƿ� �߷��� ���� ��ŭ �ٴ����� �������� ��.(�߷� ���ӵ� ����)
            gravityVelocity += Time.deltaTime * Physics.gravity.y;
        }

        moveDir.y = gravityVelocity;

        charCtrl.Move(moveDir * Time.deltaTime);
        if (!isBattle)
        {
            anim.SetFloat("XDir", x);
            anim.SetFloat("YDir", z);
            anim.SetFloat("Speed", rawSpeed); //���� ���� 0~1, �� ���� 1~2
        }
        else
        {
            anim.SetBool("isBattle", isBattle);
            anim.SetFloat("XDir", x);
            anim.SetFloat("YDir", z);
            anim.SetFloat("Speed", rawSpeed);
        }
    }
    void MouseMove()
    {
        if (!MouseControl.isFocused) return;

        //InputManager�� ���ؼ� ���콺 �̵��� ��������(mouseDelta)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //���콺 �¿� �����ӿ� �°� Rotate
        transform.Rotate(0, mouseX * turnSpeed * Time.deltaTime, 0);

        Vector3 canTargetPos = canTarget.localPosition;
        //Mathf.Clamp(��, �ּҰ�, �ִ밪) : ���� ���� �ּҰ��̳� �ִ밪 ������ ���̸� �״�� ��ȯ, �ƴϸ� �ּҰ��̳� �ִ밪�� ��ȯ
        canTargetPos.y = Mathf.Clamp(canTargetPos.y + (mouseY * lookUpSpeed * Time.deltaTime), canTargetMinHeight, canTargetMaxHeight);
        canTarget.localPosition = canTargetPos;
    }
    void PlayerRollDodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRollDodge && !player.isDie && !isJump)
        {
            isRollDodge = true;
            anim.SetTrigger("RollDodge");
            UIManager.Instance.TakeStemina(20);
            player.currentStamina -= 20f;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isRollDodge = false;
        }
    }
    void Jump()
    {
        if (isJump) return;
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isRollDodge && !player.isDie && !isJump)
        {
            isJump = true;
            anim.SetTrigger("Jump");
            UIManager.Instance.TakeStemina(20);
            player.currentStamina -= 20f;
        }
    }
    public void Landing()
    {
        isJump = false;
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
