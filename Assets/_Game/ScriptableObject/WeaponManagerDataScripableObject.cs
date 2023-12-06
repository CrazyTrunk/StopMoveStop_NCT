using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponManagerData")]
public class WeaponManagerDataScripableObject : ScriptableObject
{
    public List<WeaponData> listWeapon;
    public WeaponData GetWeaponById(int weaponId)
    {
        for (int i = 0; i < listWeapon.Count; i++)
        {
            if (listWeapon[i].id == weaponId)
            {
                return listWeapon[i];
            }
        }
        return null;
    }
    public WeaponData GetWeaponByType(WeaponType weaponType)
    {
        for (int i = 0; i < listWeapon.Count; i++)
        {
            if (listWeapon[i].type == weaponType)
            {
                return listWeapon[i];
            }
        }
        return null;
    }
}
