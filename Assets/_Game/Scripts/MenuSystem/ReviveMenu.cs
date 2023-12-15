using System;
using UnityEngine;
using UnityEngine.UI;

public class ReviveMenu : Menu<ReviveMenu>
{
    [SerializeField] private Button reviveButton, exitButton;
    [SerializeField] private RectTransform imageSpin;
    [SerializeField] private float rotationSpeed;
    private float rotZ;
    private Character attacker;
    private Player player;

    private void Start()
    {
        rotZ = 0;
        exitButton.onClick.AddListener(HandleXmarkClick);
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
        LoseMenu.Instance.OnInit(LevelManager.Instance.TotalAlive, attacker.CharacterName1, player.CoinGained);
        LoseMenu.Instance.CalculateCurrentProcess(LevelManager.Instance.TotalAlive);
        GameManager.Instance.ChangeState(GameState.GAMEOVER);
        player.PlayerData.UpdateHighestRankPerMap(player.PlayerData.levelMap, LevelManager.Instance.TotalAlive);
        GameManager.Instance.UpdatePlayerData(player.PlayerData);
        GameManager.Instance.SaveToJson(player.PlayerData, FilePathGame.CHARACTER_PATH);
        AudioManager.Instance.PlaySFX(SoundType.GAMEOVER);
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
