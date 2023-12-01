
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[System.Serializable]
public class Weapon : MonoBehaviour
{
    public string weaponName;
    [SerializeField]private Bullet bullet;
    public float bonusSpeed;
    public float bonusRange;
    public WeaponType type;
    public int cost;
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