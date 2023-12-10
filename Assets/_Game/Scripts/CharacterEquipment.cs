using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    public Transform headSocket;
    public Transform mustacheSocket;
    public SkinnedMeshRenderer characterPants;
    public Transform shieldSocket;
    private GameObject currentItemOnView;
    void OnEnable()
    {
        GlobalEvents.OnShopItemClick -= EquipOnView;
        GlobalEvents.OnShopItemClick += EquipOnView;
        GlobalEvents.OnXMarkSkinShopClicked -= ShowHideSkin;
        GlobalEvents.OnXMarkSkinShopClicked += ShowHideSkin;
    }
    public void OnInit()
    {
        if (currentItemOnView != null)
        {
            Destroy(currentItemOnView);
        }
        characterPants.enabled = false;
    }
    private void ShowHideSkin()
    {
        OnInit();
    }

    public void EquipOnView(ItemData item)
    {
        OnInit();
        switch (item.itemType)
        {
            case ItemType.HAT:
                if (item is HatData hatData)
                {
                    EquipToSocket(headSocket, hatData.itemPrefab);
                }
                break;
            case ItemType.PANTS:
                if (item is PantsData pantsData)
                {
                    EquipPants(pantsData.pantsMaterial);
                }
                break;
            case ItemType.SHIELD:
                if (item is ShieldData shieldData )
                {
                    EquipToSocket(shieldSocket, shieldData.itemPrefab);
                }
                break;
            case ItemType.MUSTACHE:
                if (item is MoustacheData moustacheData)
                {
                    EquipToSocket(mustacheSocket, moustacheData.itemPrefab);
                }
                break;
            case ItemType.SET:
                break;
            default:
                break;
        }
    }
    private void EquipPants(Material material)
    {
        characterPants.enabled = true;
        characterPants.material = material;
    }
    private void EquipToSocket(Transform socket, GameObject prefab)
    {
        currentItemOnView = Instantiate(prefab, socket.position, Quaternion.identity, socket);
        currentItemOnView.transform.localRotation = prefab.transform.localRotation;
        currentItemOnView.transform.localPosition = prefab.transform.position;
    }
}