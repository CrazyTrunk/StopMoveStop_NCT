using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemManagerData")]
public class ItemManagerDataScripableObject : ScriptableObject
{
    public List<ItemData> listItem;
    public ItemData GetSkinById(int skinId)
    {
        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].id == skinId)
            {
                return listItem[i];
            }
        }
        return null;
    }
}
