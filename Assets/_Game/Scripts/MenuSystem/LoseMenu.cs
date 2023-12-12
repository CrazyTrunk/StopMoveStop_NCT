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

    [SerializeField] private Slider currentSlider;
    [SerializeField] private Slider highScoreSlider;
    private void Start()
    {
        playerData = GameManager.Instance.GetPlayerData();
        CalculateHighScore();
    }
    private void CalculateHighScore()
    {
        //sliderValue = (1 - (ranking - 1) / (totalPlayers - 1)) * maxValueSlider
        //Bước này đảo ngược tỷ lệ phần trăm
        int highestRank = playerData.GetHighestScoreByLevel(playerData.levelMap);
        float reversePercent =(float)(highestRank - 1) / LevelManager.Instance.MaxParticipants;
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
