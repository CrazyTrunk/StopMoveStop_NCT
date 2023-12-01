
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[System.Serializable]
public class Weapon : MonoBehaviour
{
    public string weaponName;
    [SerializeField]private GameObject bulletPrefab;
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

    public void ThrowWeapon(Transform spawnPoint, Character shooter, Action<Character, Character> onHit)
    {
        GameObject bulletGameObject = SpawnBullet(spawnPoint);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        bullet.OnInit(shooter, onHit);
        bullet.Shoot();
    }
    public GameObject SpawnBullet(Transform spawnBulletPoint)
    {
        return Instantiate(bulletPrefab, spawnBulletPoint.position, spawnBulletPoint.rotation);
    }
}