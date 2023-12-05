using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : Menu<LoseMenu>
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI killedByText;
    [SerializeField] TextMeshProUGUI coinGained;
    private PlayerData playerData;
    [SerializeField] Image[] ZoneImages;
    []
    private void Start()
    {
        playerData = GameManager.Instance.GetPlayerData();
    }
    public void OnInit(int rank, string killer, int coinGain)
    {
        rankText.text = $"#{rank}" ;
        killedByText.text = killer;
        coinGained.text = coinGain.ToString();
    }
    public void OnContinueClick()
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
