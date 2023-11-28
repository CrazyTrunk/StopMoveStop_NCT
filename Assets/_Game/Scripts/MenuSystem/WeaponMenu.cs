using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMenu : Menu<WeaponMenu>
{
    public void OnXmarkClick()
    {
        Hide();
        MainMenu.Show();
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
