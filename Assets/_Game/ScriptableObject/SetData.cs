using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SetData : ItemData
{
    public float rangeBonus;
    public float movespeedBonus;
    public List<ItemData> items;
    public override void ApplyBonus(Character character)
    {
    }
}