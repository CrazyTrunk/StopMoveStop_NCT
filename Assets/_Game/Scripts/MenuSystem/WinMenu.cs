using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : Menu<WinMenu>
{
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
