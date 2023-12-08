using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemManagerData")]
public class ItemManagerDataScripableObject : ScriptableObject
{
    public List<ItemData> listItem;
}
