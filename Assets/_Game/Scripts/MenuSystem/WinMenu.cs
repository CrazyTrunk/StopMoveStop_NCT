using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : Menu<LoseMenu>
{
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
