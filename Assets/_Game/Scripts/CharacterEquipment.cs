using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private Transform headSocket;
    [SerializeField] private Transform mustacheSocket;
    [SerializeField] private SkinnedMeshRenderer characterPants;
    [SerializeField] private Transform shieldSocket;
    [SerializeField] private Transform tailSocket;
    [SerializeField] private Transform wingSocket;
    [SerializeField] private Character character;
    private Dictionary<ItemType, GameObject> equippedItems = new Dictionary<ItemType, GameObject>();

    void OnEnable()
    {
        if (character is Player)
        {
            GlobalEvents.OnShopItemClick += EquipOnView;
            GlobalEvents.OnShopItemClick += DanceOnShop;

        }

    }
    private void OnDisable()
    {
        if (character is Player)
        {
            GlobalEvents.OnShopItemClick -= EquipOnView;
            GlobalEvents.OnShopItemClick -= DanceOnShop;
        }
    }
    public void OnInit()
    {
        characterPants.material = null;
        characterPants.enabled = false;
        ClearItems();
    }

    private void ClearItems()
    {
        foreach (var item in equippedItems.Values)
        {
            if (item != null)
            {
                LeanPool.Despawn(item);
            }
        }
        equippedItems.Clear();
    }
    public void DanceOnShop(ItemData item)
    {
        if (character is Player)
        {
            character.ChangeAnim(Anim.DANCE);
        }
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
                    EquipToSocket(headSocket, hatData.itemPrefab, hatData.itemType);
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
                    EquipToSocket(shieldSocket, shieldData.itemPrefab, shieldData.itemType);
                }
                break;
            case ItemType.MUSTACHE:
                if (item is MoustacheData moustacheData)
                {
                    EquipToSocket(mustacheSocket, moustacheData.itemPrefab, moustacheData.itemType);
                }
                break;
            case ItemType.SET:
                if(item is SetData setData)
                {
                    for(int i =0; i< setData.setItems.Count; i++)
                    {
                        if (setData.setItems[i] is PantsData pants)
                        {
                            EquipPants(pants.pantsMaterial);
                        }
                        if (setData.setItems[i] is HatData hat)
                        {
                            EquipToSocket(headSocket, hat.itemPrefab, hat.itemType);
                        }
                        if (setData.setItems[i] is WingsData wing)
                        {
                            EquipToSocket(wingSocket, wing.wingsPrefab, wing.itemType);
                        }
                        if (setData.setItems[i] is TailData tailData)
                        {
                            EquipToSocket(tailSocket, tailData.tailPrefab, tailData.itemType);
                        }
                        if (setData.setItems[i] is ShieldData shield)
                        {
                            EquipToSocket(shieldSocket, shield.itemPrefab, shield.itemType);
                        }
                    }
                }
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
    private void EquipToSocket(Transform socket, GameObject prefab, ItemType itemType)
    {
        GameObject item = LeanPool.Spawn(prefab, socket.position, Quaternion.identity, socket);
        item.transform.localRotation = prefab.transform.localRotation;
        item.transform.localPosition = prefab.transform.position;
        equippedItems[itemType] = item;
    }
}