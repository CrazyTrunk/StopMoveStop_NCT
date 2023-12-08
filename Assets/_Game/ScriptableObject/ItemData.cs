using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemData
{
    public int id;
    public ItemType itemType;
    public float bonusSpeed;
    public float bonusRange;
    public int cost;
    public GameObject itemPrefab;
    public Material material;
    public Sprite imageSkin;
}