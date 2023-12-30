using Lean.Pool;
using System.Collections;
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
    [SerializeField] private WeaponManagerDataScripableObject weaponDataSO;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject warning;
    private bool warningActive = false;
    PlayerData playerData;


    private WeaponData currentEquipData;
    private WeaponData currentWeaponDataOnView;

    //PlayerData
    private int currentWeaponIndex = 0;
    private GameObject currentWeaponPrefab;

    private void OnDisable()
    {
        Hide();
    }
    public void OnInit()
    {
        playerData = GameManager.Instance.GetPlayerData();
        currentEquipData = weaponDataSO.GetWeaponById(playerData.equippedWeaponId);
        playerData = GameManager.Instance.GetPlayerData();
        coinText.text = playerData.coin.ToString();
        warning.SetActive(false);
        LoadWeapon((int)WeaponType.HAMMER);
    }
    private void DisplayButtons()
    {
        bool weaponOwned = playerData.weapons.Any(w => w == currentWeaponDataOnView.id);

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
        selectText.text = currentEquipData.type == currentWeaponDataOnView.type ? "Equipped" : "Select";
    }
    #region Buttons
    public void OnXmarkClick()
    {
        GameManager.Instance.ChangeState(GameState.MAIN_MENU);
        MainMenu.Instance.OnInit();
    }

    public void OnNextButtonClicked()
    {
        if (currentWeaponIndex == weaponDataSO.listWeapon.Count - 1)
        {
            return;
        }
        LoadWeapon(++currentWeaponIndex);
    }
    public void OnSelectButtonClicked()
    {
        playerData.equippedWeaponId = currentWeaponDataOnView.id;
        currentEquipData = weaponDataSO.GetWeaponById(playerData.equippedWeaponId);
        GameManager.Instance.UpdatePlayerData(playerData);
        DisplayButtons();
        GlobalEvents.WeaponSelected(playerData);
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);

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
        if(playerData.coin >= currentWeaponDataOnView.cost)
        {
            playerData.coin -= currentWeaponDataOnView.cost;
            BuyWeapon(currentWeaponDataOnView.id);
            coinText.text = playerData.coin.ToString();
            GameManager.Instance.UpdatePlayerData(playerData);
            DisplayButtons();
            GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
        }
        else
        {
            StartCoroutine(ShowWarning());
        }
    }
    public IEnumerator ShowWarning()
    {
        if (warningActive)
        {
            yield break;
        }
        warningActive = true;
        warning.SetActive(true);
        yield return new WaitForSeconds(1f);
        warning.SetActive(false);
        warningActive = false;

    }
    public void OnAdsButtonClick()
    {
        BuyWeapon(currentWeaponDataOnView.id);
        GameManager.Instance.UpdatePlayerData(playerData);
        DisplayButtons();
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
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
            LeanPool.Despawn(currentWeaponPrefab);
        }
    }
    public void LoadWeapon(int index)
    {
        if (index >= 0 && index < weaponDataSO.listWeapon.Count)
        {
            DestroyCurrentWeaponOnScene();
            currentWeaponPrefab = LeanPool.Spawn(weaponDataSO.listWeapon[index].weaponPrefab, spawnPoint);
            Weapon weaponPrefab = currentWeaponPrefab.GetComponent<Weapon>();
            weaponPrefab.isDemo = true;
            currentWeaponDataOnView = weaponDataSO.listWeapon[index];
            nameWeapon.text = weaponDataSO.listWeapon[index].weaponName;
            textBonus.text = $"+ {weaponDataSO.listWeapon[index].bonusRange} Range\n + {weaponDataSO.listWeapon[index].bonusSpeed} Speed";
            costText.text = weaponDataSO.listWeapon[index].cost.ToString();
            DisplayButtons();
        }
    }
    private void BuyWeapon(int weaponId)
    {
        playerData.weapons.Add(weaponId); 
    }
    #endregion
}
