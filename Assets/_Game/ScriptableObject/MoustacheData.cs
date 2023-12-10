
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/MoustacheData")]
public class MoustacheData : ItemData
{
    public float rangeBonus;
    public GameObject itemPrefab;
    public override void ApplyBonus(Character character)
    {
        character.Range += (rangeBonus / 10);
    }
}