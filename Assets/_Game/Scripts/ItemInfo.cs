
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public int id;
    public ItemType type;
    public string itemName;
    public string itemDescription;
    public string price;
    public bool isUnlocked;
    public bool isFocus;
    public bool isSelected;
    public Image image;
    public Button button;
    public List<GameObject> borders;
    public GameObject lockObject;
    public GameObject equipObject;
    protected Action<string, string, ItemInfo> onElementClick;
    public ItemData currentItem;

    public void OnInit(Action<string, string, ItemInfo> onElementClick)
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
    public void SetLock(bool locked)
    {
        isUnlocked = locked;
        UpdateLock();
    }

    private void UpdateLock()
    {
        throw new NotImplementedException();
    }

    public void HandleButtonEvent()
    {
        onElementClick?.Invoke(price, itemDescription, this);
        GlobalEvents.ShopItemClicked(currentItem);
        SkinMenu.Instance.SetFocusOnItem(this);
    }
}
