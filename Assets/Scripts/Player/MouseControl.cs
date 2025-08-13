using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public static bool isFocused;
    public bool isStoreOn;

    void Start()
    {
        //Application.isFocused  : ���� â�� ��Ŀ�� �Ǿ��ִ��� ����
        //����Ƽ �����Ϳ����� Game â�� Ŭ���ϸ� Focus, escŰ�� ������ Focus false
        OnApplicationFocus(true);
    }

    void Update()
    {
        if (isStoreOn)
        {
            OnApplicationFocus(false);
        }

        else if (!isStoreOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnApplicationFocus(false);
            }
            else if (Input.anyKeyDown)
            {
                OnApplicationFocus(true);
            }
        }
    }

    //onApplicationFocus : ���� ���μ����� os���� ��Ŀ�� �ǰų� ��Ŀ������ ��� ��� ȣ��, �Ķ���ͷ� on���� off���� ����
    void OnApplicationFocus(bool focus)
    {
        isFocused = focus;
        if (isFocused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
