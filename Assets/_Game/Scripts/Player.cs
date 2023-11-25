using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, ICombatant
{
    [SerializeField] private GameObject floatingLevelTextPrefab;
    public void ShowFloatingText(int level)
    {
        var floatText = Instantiate(floatingLevelTextPrefab, transform.position , Quaternion.identity, transform);
        floatText.GetComponent<TextMesh>().text = $"+ {level}";
    }
}
