using TMPro;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    [SerializeField] private TextMeshProUGUI coin;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI highScore;
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
        GameManager.Instance.ChangeState(GameState.Playing);
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
