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
    public void OnInit()
    {
        rotZ = 0;
        exitButton.onClick.AddListener(HandleXmarkClick);
        reviveButton.onClick.AddListener(HandleReviveClick);
        timerInt = timerValue;
        timerFloat = (float)timerValue;
        timerText.text = timerValue.ToString();
        coroutine = CountDown();
        StartCountDown();
    }
    private void OnDisable()
    {
        exitButton.onClick.RemoveListener(HandleXmarkClick);
        reviveButton.onClick.RemoveListener(HandleReviveClick);
        Hide();
    }
    private void HandleReviveClick()
    {
        Hide();
        player.OnRevive(player.level);
        GameManager.Instance.ChangeState(GameState.PLAYING);
    }

    public void OnInit(Character attacker, Player player)
    {
        this.attacker  = attacker;
        this.player = player;
        OnInit();
    }
    private void HandleXmarkClick()
    {
        GameManager.Instance.ChangeState(GameState.GAMEOVER);
        LoseMenu.Instance.OnInit(LevelManager.Instance.TotalAlive, attacker.CharacterName, player.CoinGained);
        LoseMenu.Instance.CalculateCurrentProcess(LevelManager.Instance.TotalAlive);
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
