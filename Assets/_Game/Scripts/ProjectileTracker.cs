using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTracker : MonoBehaviour
{
    public Vector3 targetPoint;
    public bool hasReached = false;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!hasReached && rb.velocity.magnitude > 0)
        {
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                hasReached = true;
                DestroyWeapon();
            }
        }
    }
    public void SetTargetPoint(Vector3 target)
    {
        targetPoint = target;
    }
    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!enemy.IsDead)
            {
                enemy.OnDeath();
                DestroyWeapon();
            }
        
        }
    }
}
