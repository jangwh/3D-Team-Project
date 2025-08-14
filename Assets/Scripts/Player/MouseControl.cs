using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public static bool isFocused;
    void Start()
    {
        //Application.isFocused  : 현재 창이 포커스 되어있는지 여부
        //유니티 에디터에서는 Game 창을 클릭하면 Focus, esc키를 누르면 Focus false
        OnApplicationFocus(true);
    }

    void Update()
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

    //onApplicationFocus : 게임 프로세스가 os에서 포커스 되거나 포커스에서 벗어날 경우 호출, 파라미터로 on인지 off인지 전달
    public void OnApplicationFocus(bool focus)
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
