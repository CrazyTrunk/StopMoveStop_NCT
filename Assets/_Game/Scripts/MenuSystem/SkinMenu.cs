
using Lean.Pool;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : Menu<SkinMenu>
{
    [SerializeField] private ItemManagerDataScripableObject itemSO;
    [SerializeField] private Button Hat, Pants, Shield, Set, buyButton, adsButton, selectButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI descriptionBonus;
    [SerializeField] private TextMeshProUGUI adsText;
    [SerializeField] public Transform parentHolder;
    [SerializeField] private Color defaultTabColor;
    [SerializeField] private Color selectedTabColor;
    [SerializeField] private GameObject itemHolder;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI selectText;
    private List<GameObject> itemHolders = new();

    private PlayerData playerData;

    private Button currentSelectedTab;

    private ItemInfo currentItemOnViewClick;
    private void SetTabColor(Button tabButton, bool isSelected)
    {
        tabButton.GetComponent<Image>().color = isSelected ? selectedTabColor : defaultTabColor;
    }
    private void SelectTab(Button selectedTab)
    {
        if (currentSelectedTab != null)
        {
            SetTabColor(currentSelectedTab, false);
        }
        SetTabColor(selectedTab, true);
        currentSelectedTab = selectedTab;
    }

    public void DisplayButtons()
    {
        bool weaponOwned = playerData.skins.Any(w => w == currentItemOnViewClick.id);

        if (weaponOwned)
        {
            buyButton.gameObject.SetActive(false);
            adsButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            IsCurrentSkinSelect();
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            adsButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
        }
    }
    public void IsCurrentSkinSelect()
    {
        selectText.text = playerData.equippedSkinId == currentItemOnViewClick.id ? "UnEquipped" : "Select";
    }
    public void OnInit()
    {
        playerData = GameManager.Instance.GetPlayerData();
        itemSO.listItem = itemSO.listItem.OrderBy(x => x.id).ToList();
        InitItems();
        Hat.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.HAT, ItemType.MUSTACHE }, Hat));
        Pants.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.PANTS }, Pants));
        Shield.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.SHIELD }, Shield));
        Set.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.SET }, Set));
        Hat.onClick.Invoke();
        coinText.text = playerData.coin.ToString();
        DisplayButtons();
        buyButton.onClick.AddListener(HandleBuy);
        selectButton.onClick.AddListener(OnSelectButtonClicked);

    }

    private void HandleBuy()
    {
        if (playerData.coin >= currentItemOnViewClick.price)
        {
            playerData.coin -= currentItemOnViewClick.price;
            BuySkin(currentItemOnViewClick.id);
            coinText.text = playerData.coin.ToString();
            currentItemOnViewClick.isUnlocked = true;
            currentItemOnViewClick.SetLock(currentItemOnViewClick.isUnlocked);
            GameManager.Instance.UpdatePlayerData(playerData);
            DisplayButtons();
            GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
        }
    }

    private void BuySkin(int id)
    {
        if (!playerData.skins.Any(w => w == id))
        {
            playerData.skins.Add(id);
        }
    }
    public void OnSelectButtonClicked()
    {
        if (playerData.equippedSkinId == currentItemOnViewClick.id)
        {
            playerData.equippedSkinId = -1;
            currentItemOnViewClick.isSelected = false;
        }
        else
        {
            playerData.equippedSkinId = currentItemOnViewClick.id;
            currentItemOnViewClick.isSelected = true;
        }
        SetSelectItem(currentItemOnViewClick);
        IsCurrentSkinSelect();
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
    }
    private void InitItems()
    {
        for (int i = 0; i < itemSO.listItem.Count; i++)
        {
            GameObject g = LeanPool.Spawn(itemHolder, parentHolder);
            ItemInfo item = g.GetComponent<ItemInfo>();
            item.image.sprite = itemSO.listItem[i].image;
            item.id = itemSO.listItem[i].id;
            item.type = itemSO.listItem[i].itemType;
            item.itemName = itemSO.listItem[i].itemName;
            item.name = itemSO.listItem[i].itemName;
            item.price = itemSO.listItem[i].cost;
            item.currentItem = itemSO.listItem[i];
            item.isUnlocked = playerData.skins.Contains(itemSO.listItem[i].id);
            item.isSelected = playerData.equippedSkinId == itemSO.listItem[i].id;

            DisplayBonus(itemSO.listItem[i], item);
            item.OnInit(OnPriceUpdate);
            item.SetLock(item.isUnlocked);
            item.SetEquip(item.isSelected);
            item.AddEventListenerToButton();
            itemHolders.Add(g);
        }
    }
    public void SetFocusOnItem(ItemInfo selectedItem)
    {
        foreach (var itemHolder in itemHolders)
        {
            ItemInfo itemInfo = itemHolder.GetComponent<ItemInfo>();
            if (itemInfo == selectedItem)
            {
                itemInfo.SetFocus(true);
            }
            else
            {
                itemInfo.SetFocus(false);
            }
        }
    }
    public void SetSelectItem(ItemInfo selectedItem)
    {
        foreach (var itemHolder in itemHolders)
        {
            ItemInfo itemInfo = itemHolder.GetComponent<ItemInfo>();
            if (itemInfo == selectedItem)
            {
                itemInfo.SetEquip(true);
            }
            else
            {
                itemInfo.SetEquip(false);
            }
        }
    }
    public void OnPriceUpdate(int price, string des, ItemInfo itemInfo)
    {
        costText.text = price.ToString();
        descriptionBonus.text = des;
        currentItemOnViewClick = itemInfo;
    }
    private void DisplayBonus(ItemData itemData, ItemInfo item)
    {
        if (itemData is HatData hatData)
        {
            item.itemDescription = $"{hatData.rangeBonus}% Range";
        }
        if (itemData is MoustacheData moustache)
        {
            item.itemDescription = $"{moustache.rangeBonus}% Range";
        }
        if (itemData is PantsData pantsData)
        {
            item.itemDescription = $"{pantsData.moveSpeedBonus}% Move Speed";
        }
        if (itemData is ShieldData shieldData)
        {
            item.itemDescription = $"Bonus Gold Inprocess...";
        }
        if (itemData is SetData setData)
        {
            if (setData.rangeBonus > 0)
            {
                item.itemDescription = $"{setData.rangeBonus}% Range";
            }
            if (setData.movespeedBonus > 0)
            {
                item.itemDescription = $"{setData.movespeedBonus}% Move Speed";
            }
        }
    }

    public void CategorizeItem(List<ItemType> itemTypes, Button button)
    {
        SelectTab(button);
        for (int i = 0; i < itemHolders.Count; i++)
        {
            if (itemTypes.Contains(itemHolders[i].GetComponent<ItemInfo>().type))
            {
                itemHolders[i].SetActive(true);
            }
            else
            {
                itemHolders[i].SetActive(false);
            }
        }
        //neu k own cái nào
        GameObject firstItem = itemHolders.FirstOrDefault(x => x.GetComponent<ItemInfo>().id == playerData.equippedSkinId);

        if (firstItem == null || !firstItem.activeSelf)
        {
            firstItem = itemHolders.FirstOrDefault(x => x.activeSelf);
        }

        if (firstItem != null)
        {
            firstItem.GetComponent<Button>().onClick?.Invoke();
            DisplayButtons();
        }
        else
        {
            buyButton.gameObject.SetActive(false);
            adsButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);
            descriptionBonus.text = "";
        }
    }
    public void OnXMarkClick()
    {
        Hide();
        MainMenu.Show();
        MainMenu.Instance.OnInit();
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.ResetCameraToOriginalPosition();
        GlobalEvents.OnXMarkSelect();
    }
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
        LeanPool.DespawnAll();
    }
}
