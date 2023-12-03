using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseMenu : Menu<LoseMenu>
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI killedByText;
    public void OnInit(string rank, string killer)
    {
        rankText.text = rank;
        killedByText.text = killer;
    }
    public void OnContinueClick()
    {
        Hide();
        MainMenu.Show();
        LevelManager.Instance.OnInit();
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
