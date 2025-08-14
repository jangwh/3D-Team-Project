using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public static MouseControl Instance { get; private set; }
    public static bool isFocused;   // 게임 창 포커스 여부
    public static bool isUIMode;    // UI 모드 여부
    public bool isStoreOn;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        isUIMode = false;
        OnApplicationFocus(true);
    }

    void Update()
    {
        // ESC 누르면 UI 모드 토글 (게임 메뉴 예시)
        if(isStoreOn)
        {
            OnApplicationFocus(false);
        }
        else if(!isStoreOn)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                ToggleUIMode(!isUIMode);
                OnApplicationFocus(false);
            }
            else if(Input.anyKeyDown)
            {
                OnApplicationFocus(true);
            }
        }
    }

    void OnApplicationFocus(bool focus)
    {
        isFocused = focus;
        ApplyCursorState();
    }

    public void ToggleUIMode(bool enable)
    {
        isUIMode = enable;
        ApplyCursorState();
    }

    private void ApplyCursorState()
    {
        if (!isFocused) // 창 포커스 잃으면 항상 커서 표시
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (isUIMode)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}