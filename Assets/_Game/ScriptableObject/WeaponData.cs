
using System;
using UnityEngine;

[Serializable]
public class WeaponData
{
    public int id;
    public string weaponName;
    public float bonusSpeed;
    public float bonusRange;
    public WeaponType type;
    public int cost;
    public GameObject weaponPrefab;
}

