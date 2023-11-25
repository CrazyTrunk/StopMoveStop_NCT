using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ThrowWeapon : MonoBehaviour
{
    [Header("Throwing")]
    [SerializeField] private float throwForce;

    public Transform attackPoint;
    private WeaponManager weaponManager;

    private void Awake()
    {
        weaponManager = WeaponManager.Instance;
    }

    public void Throw(float range, Character attacker)
    {
        GameObject weaponPrefab = weaponManager.LoadCurrentWeapon();

        GameObject projectile = Instantiate(weaponPrefab, attackPoint.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.Euler(90f, 90f, 0);
        projectile.AddComponent<Rigidbody>();
        projectile.AddComponent<ProjectileTracker>();
        projectile.AddComponent<BoxCollider>().isTrigger = true;

        Rigidbody projectRb = projectile.GetComponent<Rigidbody>();
        projectRb.useGravity = false;
        projectRb.constraints = RigidbodyConstraints.FreezePositionY;

        //5f is range
        Vector3 targetPoint = attackPoint.position + attackPoint.forward * range;
        projectRb.AddForce((targetPoint - attackPoint.position).normalized * throwForce);
        projectRb.AddTorque(transform.up * throwForce, ForceMode.Impulse);
        ProjectileTracker tracker = projectile.GetComponent<ProjectileTracker>();

        if(tracker != null)
        {
            tracker.SetTargetPoint(targetPoint, attacker);
        }
    }
}
