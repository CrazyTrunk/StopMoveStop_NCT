using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public LeanGameObjectPool botPool;
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private Joystick joystick;

    private GameObject currentLevelPrefab;
    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayerPrefab;
    private int currentLevel = 1;
    //public int maxBotsAtOnce = 5;
    //public int totalBotsToKill = 50;
    //private int currentBots = 0;
    //private int botsKilled = 0;
    //public Vector2 spawnAreaMin;
    //public Vector2 spawnAreaMax;
    //private HashSet<Vector3> usedPositions = new HashSet<Vector3>();

    private void Start()
    {
        LoadCurrentLevel(currentLevel);
        //for (int i = 0; i < maxBotsAtOnce; i++)
        //{
        //    SpawnBots(player.level);
        //}
    }

    private void LoadCurrentLevel(int currentLevel)
    {
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }
        currentLevelPrefab = Instantiate(levels[currentLevel - 1]);
        LoadPlayerResource();
    }

    //private Vector3 GenerateSpawnPosition()
    //{
    //    Vector3 potentialPosition;
    //    do
    //    {
    //        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
    //        float z = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
    //        potentialPosition = new Vector3(x, 0.08f, z); // '0' is the y-coordinate on the plane
    //    }
    //    while (usedPositions.Contains(potentialPosition));
    //    if (potentialPosition != Vector3.zero)
    //    {
    //        usedPositions.Add(potentialPosition);

    //    }
    //    return potentialPosition;
    //}
    //public void SpawnBots()
    //{
    //    Vector3 spawnPosition = GenerateSpawnPosition();
    //    Enemy enemy = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform).GetComponent<Enemy>();
    //    enemy.ResetState();
    //    enemy.InitLevelBot(player.level + Random.Range(3, 5 + 1));
    //    // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
    //    currentBots++;
    //}
    //public void SpawnBots(int level)
    //{
    //    Vector3 spawnPosition = GenerateSpawnPosition();
    //    Enemy enemy = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform).GetComponent<Enemy>();
    //    enemy.ResetState();
    //    enemy.InitLevelBot(level);
    //    // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
    //    currentBots++;
    //}
    //public void BotKilled(Character character)
    //{
    //    botsKilled++;
    //    currentBots--;
    //    usedPositions.Remove(character.transform.position);
    //    botPool.Despawn(character.gameObject);

    //    if (currentBots < maxBotsAtOnce && (totalBotsToKill - botsKilled) > 0)
    //    {
    //        SpawnBots();
    //    }
    //}
    private void LoadPlayerResource()
    {
        if (currentPlayerPrefab != null)
        {
            Destroy(currentPlayerPrefab);
        }
        currentPlayerPrefab = Instantiate(playerPrefab);
        PlayerController player = currentPlayerPrefab.GetComponent<PlayerController>();
        player.InitJoyStick(joystick);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.OnInit(currentPlayerPrefab.transform);
    }
}