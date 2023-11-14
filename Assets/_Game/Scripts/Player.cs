using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public RaidalTrigger radicalTrigger;
    [SerializeField] public GameObject Weapon;
    public void LookAtTarget(Enemy enemy)
    {
        transform.LookAt(enemy.transform.position);
    }
    public void ThrowWeapon()
    {
        throwWeapon.Throw();
    }
    public void HideWeapon()
    {
        Weapon.SetActive(false);
    }
    public void ShowWeapon()
    {
        Weapon.SetActive(true);
    }
}
