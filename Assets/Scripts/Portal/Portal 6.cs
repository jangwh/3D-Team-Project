using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal6 : MonoBehaviour
{
    public GameObject light1;
    public GameObject light2;
    public GameObject oppositePortal;
    public GameObject thismap;
    public GameObject othermap;
    public GameObject player;
    public bool isGorge;
    public bool isTown;

    public MiniMap minimap;

    public Material skyboxMaterial;

    private bool isTeleporting = false;

    void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return;

        if (other.transform.root.CompareTag("Player"))
        {
            isTeleporting = true;
            StartCoroutine(TeleportPlayer(other));
        }
        light1.SetActive(false);
        light2.SetActive(true);
    }

    IEnumerator DeactiveThisMap()
    {
        yield return null;
        thismap.SetActive(false);
    }

    IEnumerator TeleportPlayer(Collider other)
    {
        Debug.Log("플레이어가 포탈에 충돌함: " + other.name);

        oppositePortal.SetActive(true);
        othermap.SetActive(true);

        Transform playerRoot = other.transform.root;
        Rigidbody rb = playerRoot.GetComponent<Rigidbody>();

        // 충돌 방지용 포탈 콜라이더 비활성화
        Collider portalCollider = oppositePortal.GetComponent<Collider>();
        if (portalCollider != null) portalCollider.enabled = false;

        // 이동 위치 수정 (좀 더 멀리 떨어짐)
        Vector3 targetPosition = oppositePortal.transform.position + oppositePortal.transform.forward * 5f;
        Debug.Log("이동할 위치: " + targetPosition);
        Debug.Log("현재 위치 전: " + playerRoot.position);

        // Animator 비활성화 (Root Motion 방지)
        Animator anim = playerRoot.GetComponent<Animator>();
        if (anim != null) anim.applyRootMotion = false;

        // CharacterController 비활성화 (위치 덮어쓰기 방지)
        CharacterController cc = playerRoot.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        if (rb != null)
        {
            if (rb.isKinematic)
            {
                playerRoot.position = targetPosition;
                Debug.Log("isKinematic 상태에서 이동");
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.MovePosition(targetPosition);
                Debug.Log("물리 기반 이동");
            }
        }
        else
        {
            playerRoot.position = targetPosition;
            Debug.Log("Rigidbody 없음, 위치 직접 이동");
        }

        if (cc != null) cc.enabled = true;

        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
        }
        else
        {
            Debug.Log("skybox material not set");
        }

        yield return null;
        thismap.SetActive(false);
        minimap.isTown = true;
        minimap.isGorge = false;

        yield return new WaitForSeconds(1f); // 쿨타임

        if (portalCollider != null) portalCollider.enabled = true;
        isTeleporting = false;
    }
}
