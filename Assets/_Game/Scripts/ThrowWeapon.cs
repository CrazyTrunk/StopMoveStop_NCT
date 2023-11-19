using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ThrowWeapon : MonoBehaviour
{
    [Header("Throwing")]
    [SerializeField] private float throwForce;

    public GameObject weaponToThrow;
    public Transform attackPoint;

    public void Throw()
    {
        GameObject projectile = Instantiate(weaponToThrow, attackPoint.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.Euler(90f, 90f, 0);
        Rigidbody projectRb = projectile.GetComponent<Rigidbody>();
        //5f is range
        Vector3 targetPoint = attackPoint.position + attackPoint.forward * 5f;

        projectRb.AddForce((targetPoint - attackPoint.position).normalized * throwForce);
        projectRb.AddTorque(transform.up * throwForce, ForceMode.Impulse);
        ProjectileTracker tracker = projectile.GetComponent<ProjectileTracker>();

        if(tracker != null)
        {
            tracker.SetTargetPoint(targetPoint);
        }
    }
}
