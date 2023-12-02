using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenu : Menu<WeaponMenu>
{
    [SerializeField] private Button NextButton;
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button adsButton;
    [SerializeField] private Button selectButton;

    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameWeapon;
    [SerializeField] private TextMeshProUGUI status;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI textBonus;
    [SerializeField] private TextMeshProUGUI selectText;
    [SerializeField] private WeaponData weaponData;
    private Weapon currentWeaponEquip;
    private Transform spawnPoint;
    //PlayerData
    private float coin = 99999;
    private int currentWeaponIndex = 0;
    private GameObject currentWeaponPrefab;
    public void OnInit()
    {
        currentWeaponEquip = weaponData.CurrentEquipWeapon();
        spawnPoint = GameObject.Find("SpawnPointModel").transform;
        coinText.text = coin.ToString();
    }
    private void DisplayButtons()
    {
        if (WeaponData.IsUnlocked(currentWeaponPrefab.GetComponent<Weapon>().type))
        {
            status.text = "";
            buyButton.gameObject.SetActive(false);
            adsButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            IsCurrentWeaponSelect();
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            adsButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            status.text = "Lock";
        }
    }

    private void IsCurrentWeaponSelect()
    {
        selectText.text = currentWeaponEquip.type == currentWeaponPrefab.GetComponent<Weapon>().type ? "Equipped" : "Select";
    }
    #region Buttons
    public void OnXmarkClick()
    {
        Hide();
        MainMenu.Show();
        DestroyCurrentWeaponOnScene();
        currentWeaponIndex = 0;
    }

    public void OnNextButtonClicked()
    {
        if (currentWeaponIndex == weaponData.listWeapon.Count - 1)
        {
            return;
        }
        LoadWeapon(++currentWeaponIndex);
    }
    public void OnSelectButtonClicked()
    {
        if (currentWeaponEquip.type == currentWeaponPrefab.GetComponent<Weapon>().type)
        {
            return;
        }
        WeaponData.SelectWeapon(currentWeaponPrefab.GetComponent<Weapon>().type);
        currentWeaponEquip = weaponData.CurrentEquipWeapon();
        DisplayButtons();
        GlobalEvents.WeaponSelected(currentWeaponPrefab.GetComponent<Weapon>().type);
    }
    public void OnPrevButtonClicked()
    {
        if (currentWeaponIndex == 0)
        {
            return;
        }
        LoadWeapon(--currentWeaponIndex);
    }

    public void OnBuyButtonClick()
    {
        BuyWeapon(currentWeaponPrefab.GetComponent<Weapon>().type);
        DisplayButtons();
    }
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
    #endregion
    #region Functions
    private void DestroyCurrentWeaponOnScene()
    {
        if (currentWeaponPrefab != null)
        {
            Destroy(currentWeaponPrefab);
        }
    }
    public void LoadWeapon(int index)
    {
        if (index >= 0 && index < weaponData.listWeapon.Count)
        {
            DestroyCurrentWeaponOnScene();
            currentWeaponPrefab = weaponData.listWeapon[index].gameObject;
            currentWeaponPrefab = Instantiate(currentWeaponPrefab, spawnPoint);
            nameWeapon.text = weaponData.listWeapon[index].weaponName;
            textBonus.text = $"+ {weaponData.listWeapon[index].bonusRange} Range\n + {weaponData.listWeapon[index].bonusSpeed} Speed";
            costText.text = weaponData.listWeapon[index].cost.ToString();
            DisplayButtons();
        }
    }
    private void BuyWeapon(WeaponType weaponType)
    {
        WeaponData.UnlockWeapon(weaponType);
    }
    #endregion
}
