using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [SerializeField] private EnemyController controller;
    private void Start()
    {
        Weapon = WeaponShopManagerItem.Instance.GetSelectWeapon(WeaponType.HAMMER).prefabWeapon.GetComponent<Weapon>();
        InitWeaponOnHand();
        BulletPrefab = WeaponShopManagerItem.Instance.GetSelectWeapon(WeaponType.HAMMER).prefabBullet;
    }
    public void Move(Vector3 direction)
    {
        transform.position += Speed * Time.deltaTime * direction;
    }
}
