
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/ShiledData")]
public class ShieldData : ItemData
{
    public float bonusGold;
    public GameObject itemPrefab;
    public override void ApplyBonus(Character character)
    {
    }
}

