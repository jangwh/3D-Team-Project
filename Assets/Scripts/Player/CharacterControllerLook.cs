using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerLook : MonoBehaviour
{
    public float turnSpeed;
    public Transform canTarget;

    public float lookUpSpeed;

    public float canTargetMaxHeight; //canTarget이 올라갈 수 있는 최대 높이
    public float canTargetMinHeight; //canTarget이 내려갈 수 있는 최소 높이

    void Update()
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
}
