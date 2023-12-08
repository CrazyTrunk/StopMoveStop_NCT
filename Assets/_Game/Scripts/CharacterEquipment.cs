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
    public Transform pantsSocket;
    public Transform shieldSocket;
    public void Equip(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.HAT:
                break;
            case ItemType.PANTS:
                break;
            case ItemType.SHIELD:
                break;
            case ItemType.MUSTACHE:
                break;
            case ItemType.SET:
                break;
            default:
                break;
        }
    }

    private void EquipToSocket(ItemData item, Transform socket)
    {
        GameObject itemInstance = Instantiate(item.itemPrefab, socket.position, socket.rotation, socket);
    }
}