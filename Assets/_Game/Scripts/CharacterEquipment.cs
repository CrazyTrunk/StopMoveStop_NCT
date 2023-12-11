using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private Transform headSocket;
    [SerializeField] private Transform mustacheSocket;
    [SerializeField] private SkinnedMeshRenderer characterPants;
    [SerializeField] private Transform shieldSocket;
    [SerializeField] private Character character;
    private GameObject currentItemOnView;
    void OnEnable()
    {
        if (character is Player)
        {
            GlobalEvents.OnShopItemClick += EquipOnView;
        }

    }
    private void OnDisable()
    {
        if (character is Player)
        {
            GlobalEvents.OnShopItemClick -= EquipOnView;
        }
    }
    public void OnInit()
    {
        if (currentItemOnView != null)
        {
            Destroy(currentItemOnView);
        }
        characterPants.material = null;
        characterPants.enabled = false;
    }


    public void EquipOnView(ItemData item)
    {
        OnInit();
        if (item == null) return;
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
                if (item is ShieldData shieldData)
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