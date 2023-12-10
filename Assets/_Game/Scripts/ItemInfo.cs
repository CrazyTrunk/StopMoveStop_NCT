
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public ItemType type;
    public string itemName;
    public string itemDescription;
    public string price;
    public bool isUnlocked;
    public bool isFocus;
    public Image image;
    public Button button;
    public List<GameObject> borders;
    public GameObject lockObject;
    public GameObject equipObject;
    protected Action<string, string> onElementClick;
    public ItemData currentItem;

    public void OnInit(Action<string, string> onElementClick)
    {
        this.onElementClick = onElementClick;
    }
    public void UpdateBorder()
    {
        foreach (var border in borders)
        {
            if (border != null)
                border.SetActive(isFocus);
        }
    }
    public void UpdateUI()
    {

        button.onClick.AddListener(HandleButtonEvent);
    }
    public void SetFocus(bool focus)
    {
        isFocus = focus;
        UpdateBorder();
    }
    public void HandleButtonEvent()
    {
        onElementClick?.Invoke(price, itemDescription);
        GlobalEvents.ShopItemClicked(currentItem);
        SkinMenu.Instance.SetFocusOnItem(this);
    }
}
