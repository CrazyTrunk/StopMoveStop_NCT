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

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI selectText;
    [SerializeField] private WeaponData weaponData;
    private Transform spawnPoint;
    //PlayerData
    private float coin = 99999;
    private int currentWeaponIndex = 0;
    private GameObject currentWeaponPrefab;
    private void Start()
    {
        BuyWeapon();
        coinText.text = coin.ToString();
        spawnPoint = GameObject.Find("SpawnPointModel").transform;
        LoadWeapon(currentWeaponIndex);
    }

    private void DisplayButtons()
    {
        Debug.Log($"{currentWeaponPrefab.GetComponent<Weapon>().type} buy {WeaponData.IsUnlocked(currentWeaponPrefab.GetComponent<Weapon>().type)}");
        if (WeaponData.IsUnlocked(currentWeaponPrefab.GetComponent<Weapon>().type))
        {
            buyButton.gameObject.SetActive(false);
            adsButton.gameObject.SetActive(false);  
            selectButton.gameObject.SetActive(true);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            adsButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
        }
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
            DisplayButtons();
        }
    }
    private void BuyWeapon()
    {
        WeaponData.UnlockWeapon(WeaponType.HAMMER);
    }
    #endregion
}
