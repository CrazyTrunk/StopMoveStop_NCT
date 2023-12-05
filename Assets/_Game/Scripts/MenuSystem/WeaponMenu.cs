using System;
using System.Linq;
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
    [SerializeField] private Transform spawnPoint;

    PlayerData playerData;


    private Weapon currentWeaponEquip;
    private Weapon currentWeaponOnView;

    //PlayerData
    private int currentWeaponIndex = 0;
    private GameObject currentWeaponPrefab;

    public void OnInit()
    {
        playerData = GameManager.Instance.GetPlayerData();
        currentWeaponEquip = playerData.equippedWeapon;
        playerData = GameManager.Instance.GetPlayerData();
        coinText.text = playerData.coin.ToString();
    }
    private void DisplayButtons()
    {
        bool weaponOwned = playerData.weapons.Any(w => w.type == currentWeaponOnView.type);

        if (weaponOwned)
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
        selectText.text = currentWeaponEquip.type == currentWeaponOnView.type ? "Equipped" : "Select";
    }
    #region Buttons
    public void OnXmarkClick()
    {
        Hide();
        MainMenu.Show();
        MainMenu.Instance.OnInit();
        DestroyCurrentWeaponOnScene();
        currentWeaponIndex = 0;
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
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
        playerData.equippedWeapon = weaponData.GetWeaponByType(currentWeaponOnView.type);
        currentWeaponEquip = playerData.equippedWeapon;
        GameManager.Instance.UpdatePlayerData(playerData);
        DisplayButtons();
        GlobalEvents.WeaponSelected(currentWeaponOnView.type);
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
        if(playerData.coin >= currentWeaponOnView.cost)
        {
            playerData.coin -= currentWeaponOnView.cost;
            BuyWeapon(weaponData.GetWeaponByType(currentWeaponOnView.type));
            coinText.text = playerData.coin.ToString();
            GameManager.Instance.UpdatePlayerData(playerData);
            DisplayButtons();
        }
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
            currentWeaponPrefab = Instantiate(weaponData.listWeapon[index].gameObject, spawnPoint);
            currentWeaponOnView = currentWeaponPrefab.GetComponent<Weapon>();
            nameWeapon.text = weaponData.listWeapon[index].weaponName;
            textBonus.text = $"+ {weaponData.listWeapon[index].bonusRange} Range\n + {weaponData.listWeapon[index].bonusSpeed} Speed";
            costText.text = weaponData.listWeapon[index].cost.ToString();
            DisplayButtons();
        }
    }
    private void BuyWeapon(Weapon weapon)
    {
        playerData.weapons.Add(weapon); 
    }
    #endregion
}
