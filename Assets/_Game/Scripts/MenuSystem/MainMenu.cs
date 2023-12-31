﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu<MainMenu>
{
    [SerializeField] private TextMeshProUGUI coin;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button vibranceButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private GameObject vibranceOn;
    [SerializeField] private GameObject vibranceOff;
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    public delegate void MenuAction();
    public static event MenuAction OnMainMenuPlayClick;
    private PlayerData playerData;
    private void Start()
    {
        OnInit();
    }
    public void ToggleSound()
    {
        if (playerData.isSoundOn)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
    }
    public void ToggleVibrance()
    {
        if (playerData.isVibrance)
        {
            vibranceOn.SetActive(true);
            vibranceOff.SetActive(false);
        }
        else
        {
            vibranceOn.SetActive(false);
            vibranceOff.SetActive(true);
        }
    }
    private void HandleSoundButton()
    {
        playerData.isSoundOn = !playerData.isSoundOn;
        ToggleSound();
        GameManager.Instance.UpdatePlayerData(playerData);
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
        AudioManager.Instance.ToggleMusicAndSound(playerData.isSoundOn);
    }

    private void HandleVibranceButton()
    {
        playerData.isVibrance = !playerData.isVibrance;
        ToggleVibrance();
        GameManager.Instance.UpdatePlayerData(playerData);
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
    }
    private void OnEnable()
    {
        vibranceButton.onClick.AddListener(HandleVibranceButton);
        soundButton.onClick.AddListener(HandleSoundButton);
    }
    private void OnDisable()
    {
        vibranceButton.onClick.RemoveAllListeners();
        soundButton.onClick.RemoveAllListeners();
        Hide();
    }
    public void OnInit()
    {
        playerData = GameManager.Instance.GetPlayerData();
        coin.text = playerData.coin.ToString();
        inputField.text = playerData.playerName;
        highScore.text = $"Zone:{playerData.levelMap} - Best:#{GetHighScore()}";
        ToggleSound();
        ToggleVibrance();
    }

    private int GetHighScore()
    {
        int score = playerData.GetHighestScoreByLevel(playerData.levelMap);
        if (score == 0)
        {
            //neu ma chua no dc lich su nao
            score = LevelManager.Instance.CurrentLevelData.TotalBotsToKill;
        }
        return score;
    }

    public void OnPlayButtonClick()
    {
        GameManager.Instance.ChangeState(GameState.PLAYING);
        OnMainMenuPlayClick?.Invoke();
        IngameMenu.Instance.InitAliveText(LevelManager.Instance.TotalAlive);
    }
    public void OnShopMenuClick()
    {
        GameManager.Instance.ChangeState(GameState.WEAPON_MENU);
        WeaponMenu.Instance.OnInit();
    }
    public void OnSkinMenuClick()
    {
        GameManager.Instance.ChangeState(GameState.SHOP_MENU);
        SkinMenu.Instance.OnInit();
    }
    public void HandleInputEnd()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            playerData.playerName = inputField.text;
            GameManager.Instance.UpdatePlayerData(playerData);
            GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
        }

    }
    public void OnMenuLevelSelected()
    {
        Hide();
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
