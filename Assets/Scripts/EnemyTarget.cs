using UnityEngine;

public class EnemyTarget : Character
{
    public Transform lockOnPoint;
    public Collider StunCollider;
    public bool isStun;
    protected override void Start()
    {
        base.Start();
        StunCollider.enabled = false;
    }
    public override void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
    void Update()
    {
        Stun();
    }
    void Stun()
    {
        if (isStun)
        {
            StunCollider.enabled = true;
        }
        else
        {
            StunCollider.enabled = false;
        }
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