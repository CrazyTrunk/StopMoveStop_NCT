using System;
using UnityEngine;

[Serializable]
public class PantsData : ItemData
{
    public float moveSpeedBonus;
    public Material material;
    public override void ApplyBonus(Character character)
    {
        character.Speed += moveSpeedBonus;
    }
}