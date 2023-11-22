using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public LeanGameObjectPool botPool;
    public int maxBotsAtOnce = 5;
    public int totalBotsToKill = 50;
    private int currentBots = 0;
    private int botsKilled = 0;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();

    private void Awake()
    {

        for (int i = 0; i < maxBotsAtOnce; i++)
        {
            SpawnBots();

        }
    }
    private Vector3 GenerateSpawnPosition()
    {
        Vector3 potentialPosition;
        do
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float z = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            potentialPosition = new Vector3(x, 0, z); // '0' is the y-coordinate on the plane
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
        GameObject bot = botPool.Spawn(spawnPosition, Quaternion.identity, botPool.transform);
        // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
        currentBots++;
        Enemy enemy = bot.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnSpawn();
        }
    }
    public void BotKilled(ICombatant combatant)
    {
        botsKilled++;
        currentBots--;
        usedPositions.Remove(combatant.GetTransform().position);
        botPool.Despawn(combatant.GetTransform().gameObject);

        if (currentBots < maxBotsAtOnce && (totalBotsToKill - botsKilled) > 0)
        {
            SpawnBots();
        }
    }
    public void RemoveBot(GameObject bot)
    {
        botPool.Despawn(bot);
    }
}