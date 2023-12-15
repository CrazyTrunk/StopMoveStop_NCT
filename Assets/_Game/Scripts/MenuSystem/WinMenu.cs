using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : Menu<WinMenu>
{
    private PlayerData playerData;
    [SerializeField] private Sprite[] ZoneImages;
    [SerializeField] private Image currentZone;
    [SerializeField] private Image nextZone;
    [SerializeField] private TextMeshProUGUI currentZoneText;
    [SerializeField] private TextMeshProUGUI nextZoneText;
    [SerializeField] private TextMeshProUGUI coinGained;

    private void Start()
    {
        OnInit();
        currentZoneText.text = $"Zone {playerData.levelMap - 1}";
        nextZoneText.text = $"Zone {playerData.levelMap}";
        currentZone.sprite = ZoneImages[playerData.levelMap - 2];
        nextZone.sprite = ZoneImages[playerData.levelMap - 1];
        coinGained.text = LevelManager.Instance.CurrentPlayerData.CoinGained.ToString();
    }
    public void OnInit()
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
        GameManager.Instance.ChangeState(GameState.MENU);
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
