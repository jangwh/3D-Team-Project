using UnityEngine;

public class EnemyTarget : Character
{
    public Transform lockOnPoint;
    protected override void Start()
    {
        base.Start();
    }
    public override void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
    void OnDrawGizmos()
    {
        if (lockOnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lockOnPoint.position, 0.2f);
        }
    }
}