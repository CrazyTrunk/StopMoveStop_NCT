using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopManagerItem : Singleton<WeaponShopManagerItem>
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform parentSpawn;


    private GameObject currentWeaponPrefabInstance;
    private WeaponOnShop currentWeaponOnShop;
    private int currentDataIndex = 0;
    public void BuyItem(WeaponType weaponType)
    {
        weaponData.UnlockWeapon(weaponType);
    }
    public bool IsUnlockItem(WeaponType weaponType)
    {
        return weaponData.IsUnlocked(weaponType);
    }
    public void LoadFistItemInList()
    {
        currentDataIndex = 0;
        currentWeaponOnShop = weaponData.allWeapons[currentDataIndex];
        SpawnWeaponPrefab(currentWeaponOnShop);
    }
    public WeaponOnShop GetCurrentWeaponOnShop()
    {
        return currentWeaponOnShop;
    }
    public void NextItemInList()
    {

        if (currentDataIndex < weaponData.allWeapons.Count - 1)
        {
            currentDataIndex++;
            currentWeaponOnShop = weaponData.allWeapons[currentDataIndex];
            SpawnWeaponPrefab(currentWeaponOnShop);
        }
    }
    public void PrevItemInList()
    {
        if (currentDataIndex > 0)
        {
            currentDataIndex--;
            currentWeaponOnShop = weaponData.allWeapons[currentDataIndex];
            SpawnWeaponPrefab(currentWeaponOnShop);
        }
    }
    public void DestroyCurrentPrefab()
    {
        if (currentWeaponPrefabInstance != null)
        {
            Destroy(currentWeaponPrefabInstance);
        }
    }
    private void SpawnWeaponPrefab(WeaponOnShop weapon)
    {
        DestroyCurrentPrefab();
        currentWeaponPrefabInstance = Instantiate(weapon.prefabWeapon, parentSpawn.transform);
        currentWeaponPrefabInstance.transform.LookAt(parentSpawn.transform);
    }
}