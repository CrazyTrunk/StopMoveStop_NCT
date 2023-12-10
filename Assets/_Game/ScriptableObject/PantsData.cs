using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/PantsData")]
public class PantsData : ItemData
{
    public float moveSpeedBonus;
    public Material pantsMaterial;
    public override void ApplyBonus(Character character)
    {
        character.Speed += moveSpeedBonus;
    }
}