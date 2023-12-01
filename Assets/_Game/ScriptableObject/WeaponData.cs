using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/Weapons")]
public class WeaponData : ScriptableObject
{
    public List<Weapon> listWeapon;
    private const string Prefix = "Weapon_";
    private const string SelectedWeapon = "SelectedWeapon";
    public void SelectWeapon(WeaponType weaponType)
    {
        PlayerPrefs.SetInt(SelectedWeapon, (int)weaponType);
    }
    public Weapon GetSelectedWeapon()
    {
        int skinIndex = PlayerPrefs.GetInt(SelectedWeapon, 0);
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
    public void UnlockWeapon(WeaponType weaponType)
    {
        //da unlock hay chua
        PlayerPrefs.SetInt(Prefix + weaponType, 1);
    }
    public bool IsUnlocked(WeaponType weaponType)
    {
        return PlayerPrefs.GetInt(Prefix + weaponType, 0) == 1;
    }
}

//[System.Serializable]
//public class WeaponOnShop
//{
//    public string name;
//    public WeaponType type;
//    public int cost;
//    public GameObject prefabWeapon;
//    public GameObject prefabBullet;
//    public float bonusSpeed;
//    public float bonusRange;
//}