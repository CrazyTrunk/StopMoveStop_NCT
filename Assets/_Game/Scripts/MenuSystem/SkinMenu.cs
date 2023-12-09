
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : Menu<SkinMenu>
{
    [SerializeField] private ItemManagerDataScripableObject itemSO;
    [SerializeField] private Button Hat, Pants, Shield, Set;
    [SerializeField] public Transform parentHolder;
    private List<GameObject> itemHolders = new List<GameObject>();
    private GameObject itemHolder;
    private void Start()
    {
        InitItems();
        //Hat.onClick.AddListener(() => CategorizeItem(ItemType.HAT));
        //Pants.onClick.AddListener(() => CategorizeItem(ItemType.PANTS));
        //Shield.onClick.AddListener(() => CategorizeItem(ItemType.SHIELD));
        //Set.onClick.AddListener(() => CategorizeItem(ItemType.SET));
    }

    private void InitItems()
    {
        for (int i =0; i < itemSO.listItem.Count; i++)
        {
            GameObject g = Instantiate(itemHolder, parentHolder);
            itemHolders.Add(g);
            Image image = g.AddComponent<Image>();
            Button button = g.AddComponent<Button>();

            //image.sprite = itemSO.listItem[i].
        }
    }

    public void CategorizeItem(ItemType itemType)
    {
        //for (int i = 0; i < itemHolders.Count; i++)
        //{
        //    if (itemHolders[i].GetComponent<ItemHolder>().type == itemType)
        //    {
        //        itemHolders[i].SetActive(true);
        //    }
        //    else
        //    {
        //        itemHolders[i].SetActive(false);
        //    }
        //}
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
