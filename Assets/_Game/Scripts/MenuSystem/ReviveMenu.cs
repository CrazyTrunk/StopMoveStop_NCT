using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveMenu : Menu<ReviveMenu>
{
    [SerializeField] private Button reviveButton, exitButton;
    [SerializeField] private RectTransform imageSpin;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int timerValue;

    private float rotZ;
    private Character attacker;
    private Player player;
    private int timerInt;
    private float timerFloat;
    IEnumerator coroutine;
    private void Start()
    {
        OnInit();
        StartCountDown();
    }
    public void OnInit()
    {
        rotZ = 0;
        exitButton.onClick.AddListener(HandleXmarkClick);
        reviveButton.onClick.AddListener(HandleReviveClick);
        timerInt = timerValue;
        timerFloat = (float)timerValue;
        timerText.text = timerValue.ToString();
        coroutine = CountDown();
    }
    private void OnDisable()
    {
        exitButton.onClick.RemoveListener(HandleXmarkClick);
        reviveButton.onClick.RemoveListener(HandleReviveClick);
    }
    private void HandleReviveClick()
    {
        Hide();
        player.OnRevive();
        GameManager.Instance.ChangeState(GameState.PLAYING);
    }

    public void OnInit(Character attacker, Player player)
    {
        this.attacker  = attacker;
        this.player = player;
    }
    private void HandleXmarkClick()
    {
        Hide();
        LoseMenu.Show();
        LoseMenu.Instance.OnInit(LevelManager.Instance.TotalAlive, attacker.CharacterName, player.CoinGained);
        LoseMenu.Instance.CalculateCurrentProcess(LevelManager.Instance.TotalAlive);
        GameManager.Instance.ChangeState(GameState.GAMEOVER);
        player.PlayerData.UpdateHighestRankPerMap(player.PlayerData.levelMap, LevelManager.Instance.TotalAlive);
        GameManager.Instance.UpdatePlayerData(player.PlayerData);
        GameManager.Instance.SaveToJson(player.PlayerData, FilePathGame.CHARACTER_PATH);
        AudioManager.Instance.PlaySFX(SoundType.GAMEOVER);
    }
    public void StartCountDown()
    {
        StartCoroutine(coroutine);
    }
    public void StopCountDown()
    {
        HandleXmarkClick();
    }
    IEnumerator CountDown()
    {
        while (timerFloat > 0)
        {
            timerFloat -= Time.deltaTime;
            if(timerInt > (int)timerFloat)
            {
                timerText.text  = timerInt.ToString();
                timerInt -= 1;
            }
            yield return null;
        }
        StopCountDown();
    }
    private void Update()
    {
        rotZ += -Time.deltaTime * rotationSpeed;
        imageSpin.rotation = Quaternion.Euler(0,0, rotZ);
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
