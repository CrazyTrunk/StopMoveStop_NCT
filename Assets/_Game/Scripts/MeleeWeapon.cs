using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Enemy") && isThrown)
        //{
        //    Enemy enemy = other.GetComponent<Enemy>();
        //    enemy.EnemyKilled();
        //    enemy.DeactiveEnemy();
        //    DestroyWeapon();
        //}
    }
}
