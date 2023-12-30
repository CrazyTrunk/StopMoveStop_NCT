using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : Menu<WinMenu>
{
    [SerializeField] private Sprite[] ZoneImages;
    [SerializeField] private Image currentZone;
    [SerializeField] private Image nextZone;
    [SerializeField] private TextMeshProUGUI currentZoneText;
    [SerializeField] private TextMeshProUGUI nextZoneText;
    [SerializeField] private TextMeshProUGUI coinGained;
    private PlayerData playerData;

    public void OnInit()
    {
        playerData = GameManager.Instance.GetPlayerData();
        currentZoneText.text = $"Zone {playerData.levelMap - 1}";
        nextZoneText.text = $"Zone {playerData.levelMap}";
        currentZone.sprite = ZoneImages[playerData.levelMap - 2];
        nextZone.sprite = ZoneImages[playerData.levelMap - 1];
        coinGained.text = LevelManager.Instance.CurrentPlayerData.CoinGained.ToString();
    }
    private void OnDisable()
    {
        Hide();
    }
    public void OnScreenShotClick()
    {
        string filename = $"{playerData.playerName}-Level {playerData.levelMap}.png";

        ScreenCapture.CaptureScreenshot(filename);
    }
    public void OnNextZoneClicked()
    {
        GameManager.Instance.ChangeState(GameState.MAIN_MENU);
        MainMenu.Instance.OnInit();
        LevelManager.Instance.OnInit();
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
