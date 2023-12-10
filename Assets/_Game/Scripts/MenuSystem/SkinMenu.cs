
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : Menu<SkinMenu>
{
    [SerializeField] private ItemManagerDataScripableObject itemSO;
    [SerializeField] private Button Hat, Pants, Shield, Set, costButton, adsButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI descriptionBonus;
    [SerializeField] private TextMeshProUGUI adsText;
    [SerializeField] public Transform parentHolder;
    private List<GameObject> itemHolders = new List<GameObject>();
    [SerializeField] private GameObject itemHolder;

    private Button currentSelectedTab;
    [SerializeField] private Color defaultTabColor; 
    [SerializeField] private Color selectedTabColor;
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
    private void Start()
    {
        itemSO.listItem = itemSO.listItem.OrderBy(x => x.id).ToList();
        InitItems();
        Hat.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.HAT, ItemType.MUSTACHE }, Hat));
        Pants.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.PANTS }, Pants));
        Shield.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.SHIELD }, Shield));
        Set.onClick.AddListener(() => CategorizeItem(new List<ItemType> { ItemType.SET }, Set));
        Hat.onClick.Invoke();
    }

    private void InitItems()
    {
        for (int i = 0; i < itemSO.listItem.Count; i++)
        {
            GameObject g = Instantiate(itemHolder, parentHolder);
            ItemInfo item = g.GetComponent<ItemInfo>();
            item.image.sprite = itemSO.listItem[i].image;
            item.type = itemSO.listItem[i].itemType;
            item.itemName = itemSO.listItem[i].itemName;
            item.name = itemSO.listItem[i].itemName;
            item.price = itemSO.listItem[i].cost.ToString();
            DisplayBonus(itemSO.listItem[i], item);
            item.OnInit(OnPriceUpdate);
            item.UpdateUI();
            itemHolders.Add(g);
        }
        //itemHolders[0].GetComponent<Button>().onClick.Invoke();
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
    public void OnPriceUpdate(string price, string des)
    {
        costText.text = price;
        descriptionBonus.text = des;
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
            item.itemDescription = $"{5}Vi du% Gold";
        }
    }

    public void CategorizeItem(List<ItemType> itemTypes , Button button)
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

    }
    public void OnXMarkClick()
    {
        Hide();
        MainMenu.Show();
        MainMenu.Instance.OnInit();
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.ResetCameraToOriginalPosition();
    }
    public void OnInit()
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
