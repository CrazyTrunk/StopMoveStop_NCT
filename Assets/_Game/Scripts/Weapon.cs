using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Weapon : MonoBehaviour
{
    [Header("Throwing")]
    [SerializeField] private float throwForce;
    public Transform attackPoint;
    private GameObject InitProjectile(GameObject bulletPrefab , float chracterScale)
    {
        GameObject projectile = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        projectile.transform.localScale *= chracterScale;
        projectile.transform.rotation = Quaternion.Euler(90f, 90f, 0);
        projectile.AddComponent<Rigidbody>();
        projectile.AddComponent<BoxCollider>().isTrigger = true;
        return projectile;
    }
    public virtual void Throw(float range, Character attacker, float characterScale)
    {
        //GameObject weaponPrefab = weaponManager.LoadCurrentWeapon();

        //GameObject bullet = InitProjectile(weaponPrefab, characterScale);
        //Rigidbody projectRb = bullet.GetComponent<Rigidbody>();
        //projectRb.useGravity = false;
        //projectRb.constraints = RigidbodyConstraints.FreezePositionY;

        //Vector3 targetPoint = attackPoint.position + attackPoint.forward * range;
        //projectRb.AddForce((targetPoint - attackPoint.position).normalized * throwForce);
        //projectRb.AddTorque(transform.up * throwForce, ForceMode.Impulse);
        //ProjectileTracker tracker = bullet.GetComponent<ProjectileTracker>();

        //if(tracker != null)
        //{
        //    tracker.SetTargetPoint(targetPoint, attacker);
        //}
    }
}
