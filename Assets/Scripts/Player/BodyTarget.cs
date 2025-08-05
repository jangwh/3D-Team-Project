using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTargetMove : MonoBehaviour
{
    public LayerMask interactLayer;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        Vector3 pos;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, interactLayer))
        {
            //카메라 앞 1000 거리 이내에 콜라이더가 있을 경우.
            pos = hit.point; //Ray가 콜라이더에 부딛힌 지점

        }
        else
        {
            //하늘을 쳐다볼 경우 처럼 아무것도 물체가 없을 경우.
            pos = ray.origin + (ray.direction * 100);
        }

        transform.position = pos;

    }
}
