
using System;

[Serializable]
public class MoustacheData : ItemData
{
    public float rangeBonus;
    public override void ApplyBonus(Character character)
    {
        character.Range += rangeBonus;
    }
}