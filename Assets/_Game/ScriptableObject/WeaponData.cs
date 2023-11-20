using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/Weapons")]
public class WeaponData : ScriptableObject
{
    public List<Weapon> allWeapons;
}

[System.Serializable]
public class Weapon
{
    public string name;
    public int cost;
    public GameObject prefab;
    public bool isOwned;
    public bool isEquiqed;
}