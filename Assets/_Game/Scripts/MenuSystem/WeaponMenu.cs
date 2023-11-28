using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenu : Menu<WeaponMenu>
{
    [SerializeField] private Button NextButton;
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button buyButton;

    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private TextMeshProUGUI coinText;
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
        }
        else
        {
            buyButton.gameObject.SetActive(true);
        }
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
