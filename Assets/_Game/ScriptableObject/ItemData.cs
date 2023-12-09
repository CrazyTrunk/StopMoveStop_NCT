using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/ItemData")]
[Serializable]
public class ItemData : ScriptableObject
{
    public int id;
    public ItemType itemType;
    public int cost;
    public virtual void ApplyBonus(Character character)
    {
    }
}