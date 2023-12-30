using Lean.Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LeanGameObjectPool botPool;
    //[SerializeField] private LeanGameObjectPool indicatorPool;
    //[SerializeField] private Transform indicatorParent;
    //[SerializeField] private RectTransform indicatorCanvas;
    //private List<Indicator> activeIndicators = new List<Indicator>();

    [SerializeField] private List<GameObject> levels;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxBotsAtOnce;
    [SerializeField] private CameraFollow cameraFollow;

    private GameObject currentLevelPrefab;
    private Level currentLevelData;
    private List<Enemy> activeBots = new List<Enemy>();

    private GameObject currentPlayerPrefab;
    private Player currentPlayerData;
    private PlayerData playerData;
    private HashSet<Vector3> usedPositions = new();
    private int totalAlive;
    private int currentParticipants;
    private int maxParticipants;
    public int TotalAlive { get => totalAlive; set => totalAlive = value; }
    public Level CurrentLevelData { get => currentLevelData; set => currentLevelData = value; }
    public int MaxParticipants { get => maxParticipants; set => maxParticipants = value; }
    public Player CurrentPlayerData { get => currentPlayerData; set => currentPlayerData = value; }

    public void OnInit()
    {

        playerData = GameManager.Instance.GetPlayerData();
        LoadCurrentLevel(playerData.levelMap);
        GameManager.Instance.ChangeState(GameState.MENU);

        ClearAllBots();
        for (int i = 0; i < maxBotsAtOnce; i++)
        {
            SpawnBot(CurrentPlayerData.level);
        }
        currentParticipants = 1 + maxBotsAtOnce;
    }
    public void ClearAllBots()
    {
        botPool.DespawnAll();
        IndicatorManager.Instance.IndicatorPool.DespawnAll();
        usedPositions.Clear();
    }
    private void LoadCurrentLevel(int currentLevel)
    {
        if (currentLevelPrefab != null)
        {
            LeanPool.Despawn(currentLevelPrefab);
        }
        currentLevelPrefab = LeanPool.Spawn(levels[currentLevel - 1]);
        CurrentLevelData = currentLevelPrefab.GetComponent<Level>();
        MaxParticipants = CurrentLevelData.TotalBotsToKill;
        TotalAlive = CurrentLevelData.TotalBotsToKill;
        LoadPlayer();
    }

    private Vector3 GenerateSpawnPosition()
    {
        const float MIN_DISTANCE_FROM_PLAYER_AND_BOTS = 10f;
        bool positionValid;
        Vector3 potentialPosition;
        int attemps = 0;
        int MAX_ATTEMPS = 20;
        do
        {
            attemps++;
            positionValid = true;
            float x = Random.Range(CurrentLevelData.SpawnAreaMin.x, CurrentLevelData.SpawnAreaMax.x);
            float z = Random.Range(CurrentLevelData.SpawnAreaMin.y, CurrentLevelData.SpawnAreaMax.y);
            potentialPosition = new Vector3(x, 0.6f, z); // '0' is the y-coordinate on the plane
            if (Vector3.Distance(potentialPosition, currentPlayerPrefab.transform.position) < MIN_DISTANCE_FROM_PLAYER_AND_BOTS)
            {
                positionValid = false;
            }
            else
            {
                foreach (var bot in activeBots)
                {
                    if (Vector3.Distance(potentialPosition, bot.transform.position) < MIN_DISTANCE_FROM_PLAYER_AND_BOTS)
                    {
                        positionValid = false;
                        break;
                    }
                }
            }
        }
        while (!positionValid && attemps <= MAX_ATTEMPS);
        if (positionValid)
        {
            usedPositions.Add(potentialPosition);
        }
        return potentialPosition;
    }
    public void SpawnBot()
    {
        Vector3 spawnPosition = GenerateSpawnPosition();
        GameObject go = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform);
        Enemy enemy = go.GetComponent<Enemy>();
        activeBots.Add(enemy);
        enemy.OnInit();
        enemy.InitLevelBot(CurrentPlayerData.level + Random.Range(3, 5 + 1));
        IndicatorManager.Instance.CreateIndicator(enemy,enemy.level);
    }
    public void SpawnBot(int level)
    {
        Vector3 spawnPosition = GenerateSpawnPosition();
        GameObject go = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform);
        Enemy enemy = go.GetComponent<Enemy>();
        activeBots.Add(enemy);
        enemy.OnInit();
        enemy.InitLevelBot(level);
        IndicatorManager.Instance.CreateIndicator(enemy, enemy.level);
        // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
    }
    public void BotKilled(Character character)
    {

        TotalAlive--;
        if (IngameMenu.Instance != null)
        {
            IngameMenu.Instance.InitAliveText(TotalAlive);
        }

        var indicatorToDespawn = IndicatorManager.Instance.ActiveIndicators.FirstOrDefault(indicator => indicator.Target == character);
        if (indicatorToDespawn != null)
        {
            IndicatorManager.Instance.RemoveIndicator(indicatorToDespawn);
        }
        if (character is Enemy enemyKilled)
        {
            activeBots.Remove(enemyKilled);
            usedPositions.Remove(enemyKilled.transform.position);
            botPool.Despawn(enemyKilled.gameObject);
        }
        if (currentParticipants < MaxParticipants)
        {
            SpawnBot();
            currentParticipants++;
        }
        if (TotalAlive == 1)
        {
            if (playerData.levelMap < levels.Count)
            {
                playerData.UpdateHighestRankPerMap(playerData.levelMap, TotalAlive);
                playerData.levelMap++;
            }
            GameManager.Instance.UpdatePlayerData(playerData);
            GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
            GameManager.Instance.ChangeState(GameState.WIN);
            AudioManager.Instance.PlaySFX(SoundType.WIN);
            CurrentPlayerData.ChangeAnim(Anim.DANCE);
            WinMenu.Show();
        }
    }
    private void LoadPlayer()
    {
        if (currentPlayerPrefab != null)
        {
            Destroy(currentPlayerPrefab);
        }
        currentPlayerPrefab = Instantiate(playerPrefab);

        CurrentPlayerData = currentPlayerPrefab.GetComponent<Player>();
        PlayerController playerController = currentPlayerPrefab.GetComponent<PlayerController>();
        playerController.InitJoyStick(joystick);
        cameraFollow.OnInit(currentPlayerPrefab.transform);
    }
}