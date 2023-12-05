using System;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class MainMenu : Menu<MainMenu>
{
    [SerializeField] private TextMeshProUGUI coin;
    [SerializeField] private TextMeshProUGUI playerName;
    PlayerData playerData;
    [SerializeField] private TMP_InputField inputField;
    private void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        playerData = GameManager.Instance.GetPlayerData();
        coin.text = playerData.coin.ToString();
        inputField.text = playerData.playerName;
    }
    public void OnPlayButtonClick()
    {
        Hide();
        GameManager.Instance.ChangeState(GameState.Playing);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.SwitchCameraViewToPlayer();
        IngameMenu.Show();
        IngameMenu.Instance.OnInit(LevelManager.Instance.TotalBotsToKill);
    }
    public void OnShopMenuClick()
    {
        Hide();
        WeaponMenu.Show();
        WeaponMenu.Instance.OnInit();
        WeaponMenu.Instance.LoadWeapon((int)WeaponType.HAMMER);
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
