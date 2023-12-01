using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/Weapons")]
public class WeaponData : ScriptableObject
{
    public List<Weapon> listWeapon;
    private const string Prefix = "Weapon_";
    private const string EquiqWeapon = "EquipWeapon";
    public static void SelectWeapon(WeaponType weaponType)
    {
        PlayerPrefs.SetInt(EquiqWeapon, (int)weaponType);
    }
    public Weapon CurrentEquiqWeapon()
    {
        int skinIndex = PlayerPrefs.GetInt(EquiqWeapon, 0);
        if (skinIndex >= 0 && skinIndex < listWeapon.Count)
        {
            return listWeapon[skinIndex];
        }
        return null;
    }
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
    public static void UnlockWeapon(WeaponType weaponType)
    {
        //da unlock hay chua
        PlayerPrefs.SetInt(Prefix + weaponType, 1);
    }
    public static bool IsUnlocked(WeaponType weaponType)
    {
        return PlayerPrefs.GetInt(Prefix + weaponType, 0) == 1;
    }
}
