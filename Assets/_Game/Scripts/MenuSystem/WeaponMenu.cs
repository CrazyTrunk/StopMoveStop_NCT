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

    PlayerData playerData;


    private Weapon currentWeaponEquip;
    private Weapon currentWeaponOnView;

    private Transform spawnPoint;
    //PlayerData
    private int currentWeaponIndex = 0;
    private GameObject currentWeaponPrefab;
    public void OnInit()
    {
        currentWeaponEquip = weaponData.CurrentEquipWeapon();
        spawnPoint = GameObject.Find("SpawnPointModel").transform;
        playerData = PlayerData.ReadFromJson(FilePathGame.CHARACTER_PATH);
        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.OnInitData();
        }
        coinText.text = playerData.coin.ToString();
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
        if (currentWeaponEquip.type == currentWeaponOnView.type)
        {
            return;
        }
        WeaponData.SelectWeapon(currentWeaponOnView.type);
        currentWeaponEquip = weaponData.CurrentEquipWeapon();
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
            BuyWeapon(currentWeaponOnView.type);
            coinText.text = playerData.coin.ToString();
            PlayerData.SaveToJson(playerData,FilePathGame.CHARACTER_PATH);
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
    private void BuyWeapon(WeaponType weaponType)
    {
        WeaponData.UnlockWeapon(weaponType);
    }
    #endregion
}
