using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public Transform lockOnPoint;

    void OnDrawGizmos()
    {
        if (lockOnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lockOnPoint.position, 0.2f);
        }
    }
}