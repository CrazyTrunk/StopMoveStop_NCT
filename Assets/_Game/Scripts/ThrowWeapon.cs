using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private float throwCooldown;
    [Header("Throwing")]
    private bool isReadyToThrow = true;
    [SerializeField] private float throwForce;

    public GameObject weaponToThrow;
    public Transform attackPoint;

    public void Throw()
    {
        if (isReadyToThrow)
        {
            isReadyToThrow = false;
            GameObject projectile = Instantiate(weaponToThrow, attackPoint.position, Quaternion.identity);
            projectile.transform.rotation = Quaternion.Euler(90f, 90f, 0);
            Rigidbody projectRb = projectile.GetComponent<Rigidbody>();
            projectRb.AddForce(attackPoint.transform.forward * throwForce);
            float torqueForce = 500f; 
            projectRb.AddTorque(transform.up * torqueForce, ForceMode.Impulse);
            Invoke(nameof(ResetThrow), throwCooldown);
        }

    }
    private void ResetThrow()
    {
        isReadyToThrow = true;
    }
}