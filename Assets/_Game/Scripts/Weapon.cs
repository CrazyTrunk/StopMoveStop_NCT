
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Bullet bullet;
    public void HideWeapon()
    {
        gameObject.SetActive(false);
    }
    public void ShowWeapon()
    {
        gameObject.SetActive(true);
    }
    public void InitBullet(Bullet bullet)
    {
        this.bullet = bullet;
    }
    public void ThrowWeapon()
    {
        bullet.Shoot();
    }
}