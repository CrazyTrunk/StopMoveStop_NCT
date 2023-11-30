
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Bullet bullet;
    [SerializeField]private MeshRenderer render;
    public void HideWeapon()
    {
        render.enabled = false;
    }
    public void ShowWeapon()
    {
        render.enabled = true;
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