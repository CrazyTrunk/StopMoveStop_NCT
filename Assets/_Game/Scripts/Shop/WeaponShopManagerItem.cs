using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopManagerItem : Singleton<WeaponShopManagerItem>
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform parentSpawn;
    [SerializeField] private Player player;

    private GameObject currentWeaponPrefabInstance;
    private Weapon currentWeaponOnShop;
    private int currentDataIndex = 0;
    public void BuyItem(WeaponType weaponType)
    {
        weaponData.UnlockWeapon(weaponType);
    }
    public void SelectWeapon(WeaponType weaponType)
    {
        weaponData.SelectWeapon(weaponType);
        //player.Weapon = GetSelectWeapon().prefabWeapon.GetComponent<Weapon>();
        //player.InitWeaponOnHand();
        //player.ApplyWeaponBonuses(currentWeaponOnShop.bonusSpeed, currentWeaponOnShop.bonusRange);
    }
    public Weapon GetSelectWeapon()
    {
       return weaponData.GetSelectedWeapon();
    }
    public Weapon GetSelectWeapon(WeaponType weaponType)
    {
        return weaponData.GetWeaponByType(weaponType);
    }
    public bool IsUnlockItem(WeaponType weaponType)
    {
        return weaponData.IsUnlocked(weaponType);
    }
    public void LoadFistItemInList()
    {
        currentDataIndex = 0;
        currentWeaponOnShop = weaponData.listWeapon[currentDataIndex];
        SpawnWeaponPrefab(currentWeaponOnShop);
    }
    public Weapon GetCurrentWeaponOnShop()
    {
        return currentWeaponOnShop;
    }
    public void NextItemInList()
    {

        if (currentDataIndex < weaponData.listWeapon.Count - 1)
        {
            currentDataIndex++;
            currentWeaponOnShop = weaponData.listWeapon[currentDataIndex];
            SpawnWeaponPrefab(currentWeaponOnShop);
        }
    }
    public void PrevItemInList()
    {
        if (currentDataIndex > 0)
        {
            currentDataIndex--;
            currentWeaponOnShop = weaponData.listWeapon[currentDataIndex];
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
    private void SpawnWeaponPrefab(Weapon weapon)
    {
        DestroyCurrentPrefab();
        //currentWeaponPrefabInstance = Instantiate(weapon.prefabWeapon, parentSpawn.transform);
        currentWeaponPrefabInstance.transform.LookAt(parentSpawn.transform);
    }
}