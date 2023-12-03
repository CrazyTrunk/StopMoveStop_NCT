﻿using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public LeanGameObjectPool botPool;
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject playerPrefab;
    private PlayerData playerData;
    [SerializeField] private int maxBotsAtOnce;

    private const string playerDataTxt = "playerData.txt";
    private GameObject currentLevelPrefab;
    private Level currentLevelData;

    private GameObject currentPlayerPrefab;
    private Player currentPlayerData;

    private int currentLevel = 1;

    private int currentBots = 0;
    private int botsKilled = 0;

    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();

    private void Start()
    {
        playerData = PlayerData.ReadFromJson(playerDataTxt);
        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.OnInitData();
        }
        LoadCurrentLevel(currentLevel);
        for (int i = 0; i < maxBotsAtOnce; i++)
        {
            SpawnBots(currentPlayerData.level);
        }
    }

    private void LoadCurrentLevel(int currentLevel)
    {
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }
        currentLevelPrefab = Instantiate(levels[currentLevel - 1]);
        currentLevelData = currentLevelPrefab.GetComponent<Level>();

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
    public void SpawnBots()
    {
        Vector3 spawnPosition = GenerateSpawnPosition();
        Enemy enemy = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform).GetComponent<Enemy>();
        enemy.ResetState();
        enemy.InitLevelBot(currentPlayerData.level + Random.Range(3, 5 + 1));
        // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
        currentBots++;
    }
    public void SpawnBots(int level)
    {
        Vector3 spawnPosition = GenerateSpawnPosition();
        Enemy enemy = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform).GetComponent<Enemy>();
        enemy.ResetState();
        enemy.InitLevelBot(level);
        // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
        currentBots++;
    }
    public void BotKilled(Character character)
    {
        botsKilled++;
        currentBots--;
        usedPositions.Remove(character.transform.position);
        botPool.Despawn(character.gameObject);

        if (currentBots < maxBotsAtOnce && (currentLevelData.TotalBotsToKill - botsKilled) > 0)
        {
            SpawnBots();
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
        currentPlayerData.LoadData(playerData);
        PlayerController playerController = currentPlayerPrefab.GetComponent<PlayerController>();
        playerController.InitJoyStick(joystick);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.OnInit(currentPlayerPrefab.transform);
    }
}