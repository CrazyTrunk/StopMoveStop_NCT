using Lean.Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LeanGameObjectPool botPool;

    [SerializeField] private List<GameObject> levels;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxBotsAtOnce;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private CopyPosition copyPos;

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
        Vector3 spawnPosition;
        do
        {
            var direction = Random.insideUnitCircle.normalized;
            var distance = Random.Range(10, 15);
            var pos = direction * distance;

            spawnPosition = currentLevelData.transform.position + new Vector3(pos.x, 0.6f, pos.y);

            // Điều chỉnh vị trí nếu nằm ngoài khu vực cho phép
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, CurrentLevelData.SpawnAreaMin.x, CurrentLevelData.SpawnAreaMax.x);
            spawnPosition.z = Mathf.Clamp(spawnPosition.z, CurrentLevelData.SpawnAreaMin.y, CurrentLevelData.SpawnAreaMax.y);

        } while (usedPositions.Contains(spawnPosition)); // Kiểm tra nếu vị trí đã được sử dụng

        usedPositions.Add(spawnPosition); // Thêm vị trí vào danh sách đã sử dụng
        return spawnPosition;
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
            GameManager.Instance.ChangeState(GameState.WIN);
            GameManager.Instance.UpdatePlayerData(playerData);
            GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
            AudioManager.Instance.PlaySFX(SoundType.WIN);
            CurrentPlayerData.ChangeAnim(Anim.DANCE);
            WinMenu.Instance.OnInit();
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
        copyPos.SetTarget(currentPlayerPrefab.transform);
    }
}