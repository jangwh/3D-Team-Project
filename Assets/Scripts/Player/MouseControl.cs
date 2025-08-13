using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public static MouseControl Instance { get; private set; }
    public static bool isFocused;   // ���� â ��Ŀ�� ����
    public static bool isUIMode;    // UI ��� ����
    public bool isStoreOn;
    private bool prevStoreOn;

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
        prevStoreOn = isStoreOn;
        OnApplicationFocus(true);
    }

    void Update()
    {
        if (prevStoreOn != isStoreOn)
        {
            if (!isStoreOn)
            {
                // Store가 꺼질 때 강제로 UI 모드도 해제 후 ApplyCursorState 실행
                isUIMode = false;
                ApplyCursorState();
            }
            prevStoreOn = isStoreOn;
        }
        // ESC ������ UI ��� ��� (���� �޴� ����)
        if(isStoreOn)
        {
            OnApplicationFocus(false);
        }
        else if(!isStoreOn)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                OnApplicationFocus(false);
            }
            else if(Input.anyKeyDown)
            {
                OnApplicationFocus(true);
            }
        }
    }

    public void OnApplicationFocus(bool focus)
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
        if (!isFocused) // â ��Ŀ�� ������ �׻� Ŀ�� ǥ��
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