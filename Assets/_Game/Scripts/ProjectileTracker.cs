using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTracker : MonoBehaviour
{
    public Vector3 targetPoint;
    public bool hasReached = false;
    private Rigidbody rb;
    //Determine who shoot it
    public ICombatant Shooter { get; set; }

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
    public void SetTargetPoint(Vector3 target, ICombatant shooter)
    {
        targetPoint = target;
        Shooter = shooter;
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
                enemy.LastAttacker = Shooter;
                enemy.OnDeath();
                DestroyWeapon();
                if (enemy.LastAttacker != null && !enemy.LastAttacker.IsDead)
                {
                    enemy.LastAttacker.LevelUp(enemy.Level);
                }
            }

        }
    }
}
