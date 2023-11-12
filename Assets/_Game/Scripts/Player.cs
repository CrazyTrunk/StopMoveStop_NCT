using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private ThrowWeapon throwWeapon;
    public void Throw(Enemy enemy)
    {
        LookAtTarget(enemy);
        ChangeAnim("attack");
        throwWeapon.Throw();
    }
    public void LookAtTarget(Enemy enemy)
    {
        transform.LookAt(enemy.transform.position);
    }

}
