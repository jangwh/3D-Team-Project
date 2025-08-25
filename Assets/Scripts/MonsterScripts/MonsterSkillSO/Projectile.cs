using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private float damage;

    public void SetTarget(Transform newTarget, float newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    //충돌처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            Character character = target.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
