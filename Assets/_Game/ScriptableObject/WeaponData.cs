using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/Weapons")]
public class WeaponData : ScriptableObject
{
    public List<Weapon> listWeapon;
    public Weapon GetWeaponByType(WeaponType weaponType)
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
