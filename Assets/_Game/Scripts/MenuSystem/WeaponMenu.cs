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

    //PlayerData
    private float coin = 99999;

    private WeaponOnShop currentWeapon;
    private void Start()
    {
        coinText.text = coin.ToString();
        SetCostData();
    }
    public void OnXmarkClick()
    {
        Hide();
        MainMenu.Show();
        WeaponShopManagerItem.Instance.DestroyCurrentPrefab();
    }

    public void OnNextButtonClicked()
    {
        WeaponShopManagerItem.Instance.NextItemInList();
        SetCostData();
    }
    public void OnSelectButtonClicked()
    {
        WeaponShopManagerItem.Instance.SelectWeapon(currentWeapon.type);
        SetCostData();
    }
    public void OnPrevButtonClicked()
    {
        WeaponShopManagerItem.Instance.PrevItemInList();
        SetCostData();
    }
    public void SetCostData()
    {
        currentWeapon = WeaponShopManagerItem.Instance.GetCurrentWeaponOnShop();
        costText.text = currentWeapon.cost.ToString();
        if (WeaponShopManagerItem.Instance.IsUnlockItem(currentWeapon.type))
        {
            buyButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            adsButton.gameObject.SetActive(false);
            CheckingIfEquip();
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            adsButton.gameObject.SetActive(true);
            //buyButton.interactable = false;
        }
    }
    public void CheckingIfEquip()
    {
        selectText.text = currentWeapon.type == WeaponShopManagerItem.Instance.GetSelectWeapon().type ? "Equipped" : "Select";
    }
    public void OnBuyButtonClick()
    {
        if (coin >= currentWeapon.cost)
        {
            coin -= currentWeapon.cost;
            coinText.text = coin.ToString();
            WeaponShopManagerItem.Instance.BuyItem(currentWeapon.type);
            SetCostData();
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

}
