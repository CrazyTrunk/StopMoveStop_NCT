using System;

[Serializable]
public class HatData : ItemData
{
    public float rangeBonus;
    public override void ApplyBonus(Character character)
    {
        character.Range += rangeBonus;
    }
}