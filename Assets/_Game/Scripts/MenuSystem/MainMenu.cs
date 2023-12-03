using System;
using TMPro;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    [SerializeField] private TextMeshProUGUI coin;
    PlayerData playerData;
    private void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        playerData = PlayerData.ReadFromJson(FilePathGame.CHARACTER_PATH);
        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.OnInitData();
        }
        coin.text = playerData.coin.ToString();
    }
    public void OnPlayButtonClick()
    {
        Hide();
        GameManager.Instance.ChangeState(GameState.Playing);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.SwitchCameraViewToPlayer();
        GlobalEvents.OnPlayClicked();
    }
    public void OnShopMenuClick()
    {
        Hide();
        WeaponMenu.Show();
        WeaponMenu.Instance.OnInit();
        WeaponMenu.Instance.LoadWeapon((int)WeaponType.HAMMER);
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
