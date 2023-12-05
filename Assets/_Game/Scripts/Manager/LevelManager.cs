using Lean.Pool;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LeanGameObjectPool botPool;
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxBotsAtOnce;
    private GameObject currentLevelPrefab;
    private Level currentLevelData;

    private GameObject currentPlayerPrefab;
    private Player currentPlayerData;

    private HashSet<Vector3> usedPositions = new();
    private int totalBotsToKill;
    private int botsSpawned;

    public int TotalBotsToKill { get => totalBotsToKill; set => totalBotsToKill = value; }

    private void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        botsSpawned = 0;
        LoadCurrentLevel(GameManager.Instance.GetPlayerData().levelMap);
        GameManager.Instance.ChangeState(GameState.MainMenu);

        ClearAllBots();
        for (int i = 0; i < maxBotsAtOnce; i++)
        {
            SpawnBot(currentPlayerData.level);
        }
    }
    public void ClearAllBots()
    {
        botPool.DespawnAll();
        usedPositions.Clear();
    }
    private void LoadCurrentLevel(int currentLevel)
    {
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }
        currentLevelPrefab = Instantiate(levels[currentLevel - 1]);
        currentLevelData = currentLevelPrefab.GetComponent<Level>();
        TotalBotsToKill = currentLevelData.TotalBotsToKill;
        LoadPlayer();
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 potentialPosition;
        do
        {
            float x = Random.Range(currentLevelData.SpawnAreaMin.x, currentLevelData.SpawnAreaMax.x);
            float z = Random.Range(currentLevelData.SpawnAreaMin.y, currentLevelData.SpawnAreaMax.y);
            potentialPosition = new Vector3(x, 0.08f, z); // '0' is the y-coordinate on the plane
        }
        while (usedPositions.Contains(potentialPosition));
        if (potentialPosition != Vector3.zero)
        {
            usedPositions.Add(potentialPosition);
        }
        return potentialPosition;
    }
    public void SpawnBot()
    {
        if (botsSpawned <= TotalBotsToKill)
        {
            Vector3 spawnPosition = GenerateSpawnPosition();
            Enemy enemy = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform).GetComponent<Enemy>();
            enemy.OnInit();
            enemy.InitLevelBot(currentPlayerData.level + Random.Range(3, 5 + 1));
            botsSpawned++;
            // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
        }
    }
    public void SpawnBot(int level)
    {
        if (botsSpawned < TotalBotsToKill)
        {
            Vector3 spawnPosition = GenerateSpawnPosition();
            Enemy enemy = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform).GetComponent<Enemy>();
            enemy.OnInit();
            enemy.InitLevelBot(level);
            botsSpawned++;
        }
        // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
    }
    public void BotKilled(Character character)
    {
        TotalBotsToKill--;
        IngameMenu.Instance.OnInit(TotalBotsToKill);
        usedPositions.Remove(character.transform.position);
        botPool.Despawn(character.gameObject);

        if (TotalBotsToKill > 1)
        {
            SpawnBot();
        }
        else if (TotalBotsToKill == 1)
        {
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
        currentPlayerData = currentPlayerPrefab.GetComponent<Player>();
        currentPlayerData.OnInit();
        PlayerController playerController = currentPlayerPrefab.GetComponent<PlayerController>();
        playerController.InitJoyStick(joystick);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.OnInit(currentPlayerPrefab.transform);
    }
}