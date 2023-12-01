using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [SerializeField] private EnemyController controller;
    public override void Awake()
    {
        base.Awake();
        ChangeWeapon(WeaponType.HAMMER);
    }
    public void Move(Vector3 direction)
    {
        transform.position += Speed * Time.deltaTime * direction;
    }
}
