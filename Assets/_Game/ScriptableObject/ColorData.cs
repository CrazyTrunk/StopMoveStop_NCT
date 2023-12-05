using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Game Data/ColorData")]
public class ColorData : ScriptableObject
{
    public List<ColorGame> colors;
}
[Serializable]
public class ColorGame
{
    public ColorType colorType;
    public Color color;
    public Material material;
}