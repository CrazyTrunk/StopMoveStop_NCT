
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public WeaponData weaponData;
    private Dictionary<string, bool> ownedWeapons = new Dictionary<string, bool>();
    private void Awake()
    {
        LoadWeapons();
    }
    private void LoadWeapons()
    {
        foreach (var weapon in weaponData.allWeapons)
        {
            ownedWeapons[weapon.name] = PlayerPrefs.GetInt(weapon.name, 0) == 1;
        }
    }
    public GameObject LoadCurrentWeapon()
    {
        foreach (Weapon weapon in weaponData.allWeapons)
        {
            if (weapon.isEquiqed && weapon.isOwned)
            {
                return weapon.prefab;
            }
        }
        return null;
    }
}
