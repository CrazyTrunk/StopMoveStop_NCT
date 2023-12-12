using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : Menu<WinMenu>
{
    private PlayerData playerData;
    private void Start()
    {
        playerData = GameManager.Instance.GetPlayerData();
    }
    public void OnNextZoneClicked()
    {
        Hide();
        IngameMenu.Hide();
        MainMenu.Show();
        MainMenu.Instance.OnInit();
        LevelManager.Instance.OnInit();
        GameManager.Instance.ChangeState(GameState.MainMenu);
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
