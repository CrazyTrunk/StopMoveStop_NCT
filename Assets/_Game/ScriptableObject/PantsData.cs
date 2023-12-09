using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/PantsData")]
public class PantsData : ItemData
{
    public float moveSpeedBonus;
    public Sprite skin;
    public override void ApplyBonus(Character character)
    {
        character.Speed += moveSpeedBonus;
    }
}