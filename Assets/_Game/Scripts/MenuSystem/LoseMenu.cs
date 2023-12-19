using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : Menu<LoseMenu>
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI killedByText;
    [SerializeField] private TextMeshProUGUI coinGained;
    private PlayerData playerData;
    [SerializeField] private Sprite[] ZoneImages;
    [SerializeField] private Image currentZone;
    [SerializeField] private Image nextZone;
    [SerializeField] private TextMeshProUGUI currentZoneText;
    [SerializeField] private TextMeshProUGUI nextZoneText;

    [SerializeField] private Slider currentSlider;
    [SerializeField] private Slider highScoreSlider;
    private void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        playerData = GameManager.Instance.GetPlayerData();
        CalculateHighScore();
        currentZoneText.text = $"Zone {playerData.levelMap}";
        nextZoneText.text = $"Zone {playerData.levelMap + 1}";
        currentZone.sprite = ZoneImages[playerData.levelMap - 1];
        nextZone.sprite = ZoneImages[playerData.levelMap];
    }
    public void OnScreenShotClick()
    {
        string filename = $"{playerData.playerName}-Level {playerData.levelMap}.png";

        ScreenCapture.CaptureScreenshot(filename);
    }
    private void CalculateHighScore()
    {
        //sliderValue = (1 - (ranking - 1) / (totalPlayers - 1)) * maxValueSlider
        //Bước này đảo ngược tỷ lệ phần trăm
        int highestRank = playerData.GetHighestScoreByLevel(playerData.levelMap);
        float reversePercent = (float)(highestRank - 1) / LevelManager.Instance.MaxParticipants;
        float reverseToPercent = 1 - reversePercent;
        highScoreSlider.value = reverseToPercent * 11.5f;
    }
    public void CalculateCurrentProcess(int currentRank)
    {
        float reversePercent = (float)(currentRank - 1) / LevelManager.Instance.MaxParticipants;
        float reverseToPercent = 1 - reversePercent;
        currentSlider.value = reverseToPercent * 11.5f;
    }
    public void OnInit(int rank, string killer, int coinGain)
    {
        rankText.text = $"#{rank}";
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
