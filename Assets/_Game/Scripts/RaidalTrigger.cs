using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaidalTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MeleeWeapon"))
        {
            Weapon weapon = Cache.GetWeapon(other);
            if (weapon.isMelee)
            {
                MeleeWeapon meleeWeapon = weapon as MeleeWeapon;
                meleeWeapon.DestroyWeapon();
            }
        }
    }
}
