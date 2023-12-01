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
    //PlayerData
    private float coin = 99999;

    private GameObject currentWeaponPrefab;
    private void Start()
    {
        coinText.text = coin.ToString();
        LoadWeaponFromType(WeaponType.HAMMER);
    }

    private void LoadWeaponFromType(WeaponType weaponType)
    {
        if (currentWeaponPrefab != null)
        {
            Destroy(currentWeaponPrefab);
        }
        currentWeaponPrefab = weaponData.GetWeaponByType(weaponType).gameObject;
        Instantiate(currentWeaponPrefab);
    }

    public void OnXmarkClick()
    {
        Hide();
        MainMenu.Show();
    }

    public void OnNextButtonClicked()
    {
    }
    public void OnSelectButtonClicked()
    {
    }
    public void OnPrevButtonClicked()
    {
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

}
