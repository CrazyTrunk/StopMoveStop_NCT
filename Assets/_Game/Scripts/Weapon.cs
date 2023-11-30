
using System;
using UnityEditor.Experimental.GraphView;
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
    public void ThrowWeapon(GameObject bulletPrefab, Transform spawnPoint, Character shooter, Action<Character, Character> onHit)
    {
        GameObject bulletGameObject = SpawnBullet(bulletPrefab , spawnPoint);
        bullet = bulletGameObject.GetComponent<Bullet>();
        bullet.OnInit(shooter, onHit);
        bullet.Shoot();
    }
    public GameObject SpawnBullet(GameObject bulletPrefab, Transform spawnBulletPoint)
    {
        return Instantiate(bulletPrefab, spawnBulletPoint.position, spawnBulletPoint.rotation);
    }
}