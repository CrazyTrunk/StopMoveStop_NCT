using System;
using TMPro;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    [SerializeField] private TextMeshProUGUI coin;
    public event Action OnPlayButtonPressed;
    PlayerData playerData;
    private const string playerDataTxt = "playerData.txt";
    private void Start()
    {
        playerData = PlayerData.ReadFromJson(playerDataTxt);
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
        OnPlayButtonPressed?.Invoke();
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
