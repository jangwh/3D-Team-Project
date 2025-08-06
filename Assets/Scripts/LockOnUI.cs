using UnityEngine;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity; // UI가 회전하지 않게 고정
    }
}