using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameMenu : Menu<IngameMenu>
{
    [SerializeField] TextMeshProUGUI alive;
    public void OnSettingClick()
    {
        Debug.Log("Setting");
    }
    public void OnInit(int botAlive)
    {
        alive.text = $"Alive: {botAlive}";
    }
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
