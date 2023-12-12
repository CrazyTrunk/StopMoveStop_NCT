using Lean.Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LeanGameObjectPool botPool;
    [SerializeField] private LeanGameObjectPool indicatorPool;
    [SerializeField] private Transform indicatorParent;
    [SerializeField] private RectTransform indicatorCanvas;
    private List<Indicator> activeIndicators = new List<Indicator>();

    [SerializeField] private List<GameObject> levels;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxBotsAtOnce;
    private GameObject currentLevelPrefab;
    private Level currentLevelData;

    private GameObject currentPlayerPrefab;
    private Player currentPlayerData;
    private PlayerData playerData;
    private HashSet<Vector3> usedPositions = new();
    private int totalAlive;
    private int currentParticipants;
    private int maxParticipants;
    public int TotalAlive { get => totalAlive; set => totalAlive = value; }
    public Level CurrentLevelData { get => currentLevelData; set => currentLevelData = value; }
    public void OnInit()
    {

        playerData = GameManager.Instance.GetPlayerData();
        LoadCurrentLevel(playerData.levelMap);
        GameManager.Instance.ChangeState(GameState.MainMenu);

        ClearAllBots();
        for (int i = 0; i < maxBotsAtOnce; i++)
        {
            SpawnBot(currentPlayerData.level);
        }
        currentParticipants = 1 + maxBotsAtOnce;
    }
    public void ClearAllBots()
    {
        botPool.DespawnAll();
        indicatorPool.DespawnAll();
        usedPositions.Clear();
    }
    private void LoadCurrentLevel(int currentLevel)
    {
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }
        currentLevelPrefab = Instantiate(levels[currentLevel - 1]);
        CurrentLevelData = currentLevelPrefab.GetComponent<Level>();
        maxParticipants = CurrentLevelData.TotalBotsToKill;
        TotalAlive = CurrentLevelData.TotalBotsToKill;
        LoadPlayer();
    }

    private Vector3 GenerateSpawnPosition()
    {
        bool positionValid;
        Vector3 potentialPosition;
        do
        {
            positionValid = true;
            float x = Random.Range(CurrentLevelData.SpawnAreaMin.x, CurrentLevelData.SpawnAreaMax.x);
            float z = Random.Range(CurrentLevelData.SpawnAreaMin.y, CurrentLevelData.SpawnAreaMax.y);
            potentialPosition = new Vector3(x, 0.08f, z); // '0' is the y-coordinate on the plane

            Collider[] hitColliders = new Collider[10];
            int numColliders = Physics.OverlapSphereNonAlloc(potentialPosition, 6f, hitColliders);
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].CompareTag(Tag.CHARACTER) || hitColliders[i].CompareTag(Tag.OBSTACLE) || usedPositions.Contains(potentialPosition))
                {
                    positionValid = false;
                    break;
                }
            }
        }
        while (!positionValid);
        if (potentialPosition != Vector3.zero)
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
        enemy.OnInit();
        enemy.InitLevelBot(currentPlayerData.level + Random.Range(3, 5 + 1));
        Indicator indicator = indicatorPool.Spawn(Vector3.zero, Quaternion.identity, indicatorParent).GetComponent<Indicator>();
        indicator.SetTarget(enemy, indicatorCanvas, enemy.level);
        activeIndicators.Add(indicator);

    }
    public void SpawnBot(int level)
    {
        Vector3 spawnPosition = GenerateSpawnPosition();
        GameObject go = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.OnInit();
        enemy.InitLevelBot(level);
        Indicator indicator = indicatorPool.Spawn(spawnPosition, Quaternion.identity, indicatorParent).GetComponent<Indicator>();
        indicator.SetTarget(enemy, indicatorCanvas, enemy.level);
        activeIndicators.Add(indicator);
        // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
    }
    public void BotKilled(Character character)
    {
        TotalAlive--;
        IngameMenu.Instance.OnInit(TotalAlive);
        usedPositions.Remove(character.transform.position);
        botPool.Despawn(character.gameObject);
        if (currentParticipants < maxParticipants)
        {
            SpawnBot();
            currentParticipants++;
        }


        var indicatorToDespawn = activeIndicators.FirstOrDefault(indicator => indicator.Target == character.transform);
        if (indicatorToDespawn != null)
        {
            indicatorPool.Despawn(indicatorToDespawn.gameObject);
            activeIndicators.Remove(indicatorToDespawn);
        }

        if(TotalAlive == 1)
        {
            if (playerData.levelMap < levels.Count)
            {
                playerData.UpdateHighestRankPerMap(playerData.levelMap, TotalAlive);
                playerData.levelMap++;
                GameManager.Instance.UpdatePlayerData(playerData);
                GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
            }
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
        PlayerController playerController = currentPlayerPrefab.GetComponent<PlayerController>();
        playerController.InitJoyStick(joystick);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.OnInit(currentPlayerPrefab.transform);
    }
}