using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }
}
