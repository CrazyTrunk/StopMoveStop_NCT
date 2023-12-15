using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Toggle;

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

    private PlayerData playerData;
    private void Start()
    {
        OnInit();
        vibranceButton.onClick.AddListener(HandleVibranceButton);
        soundButton.onClick.AddListener(HandleSoundButton);
        ToggleSound();
        ToggleVibrance();
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
    }

    private void HandleVibranceButton()
    {
        playerData.isVibrance = !playerData.isVibrance;
        ToggleVibrance();
        GameManager.Instance.UpdatePlayerData(playerData);
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
    }

    private void OnDisable()
    {
        vibranceButton.onClick.RemoveAllListeners();
        soundButton.onClick.RemoveAllListeners();

    }
    public void OnInit()
    {
        GameManager.Instance.ChangeState(GameState.MENU);
        playerData = GameManager.Instance.GetPlayerData();
        coin.text = playerData.coin.ToString();
        inputField.text = playerData.playerName;
        highScore.text = $"Zone:{playerData.levelMap} - Best:#{GetHighScore()}";
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
        Hide();
        GameManager.Instance.ChangeState(GameState.PLAYING);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.SwitchCameraViewToPlayer();
        IngameMenu.Show();
        IngameMenu.Instance.OnInit(LevelManager.Instance.TotalAlive);
    }
    public void OnShopMenuClick()
    {
        Hide();
        WeaponMenu.Show();
        WeaponMenu.Instance.OnInit();
        WeaponMenu.Instance.LoadWeapon((int)WeaponType.HAMMER);
    }
    public void OnSkinMenuClick()
    {
        Hide();
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.SwitchCameraViewToSkinShop();
        SkinMenu.Show();
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
