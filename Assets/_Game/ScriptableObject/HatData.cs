using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/HatData")]
public class HatData : ItemData
{
    public float rangeBonus;
    public GameObject itemPrefab;
    public override void ApplyBonus(Character character)
    {
        character.Range += rangeBonus;
    }
}