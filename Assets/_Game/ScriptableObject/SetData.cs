using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/SetData")]
public class SetData : ItemData
{
    public float rangeBonus;
    public float movespeedBonus;
    public List<ItemData> setItems;
    public override void ApplyBonus(Character character)
    {
        character.Range += (rangeBonus / 10);
        character.Speed += (movespeedBonus / 10);
    }
}