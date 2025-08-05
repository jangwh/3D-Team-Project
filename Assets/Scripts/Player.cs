using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    Vector3 moveDir;



    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDir = new Vector3(x, 0, z);
        transform.position += moveDir * moveSpeed * Time.deltaTime;


    }
}
