
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[System.Serializable]
public class Weapon : MonoBehaviour
{
    [SerializeField]private GameObject bulletPrefab;
    [SerializeField]private WeaponType weaponType;
    [SerializeField]private MeshRenderer render;
    public bool isDemo;
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
    private void LateUpdate()
    {
        if(isDemo)
        {
            transform.Rotate(Vector3.up * 0.5f, Space.Self);
        }
    }
}