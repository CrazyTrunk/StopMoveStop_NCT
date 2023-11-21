using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public LeanGameObjectPool botPool;
    public int maxBotsAtOnce = 5;
    public int totalBotsToKill = 50;
    private int currentBots = 0;
    private int botsKilled = 0;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();

    private void Update()
    {
        if (currentBots < maxBotsAtOnce && (totalBotsToKill - botsKilled) > currentBots)
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

        usedPositions.Add(potentialPosition);
        return potentialPosition;
    }
    public void SpawnBots()
    {
        Vector3 spawnPosition = GenerateSpawnPosition();
        GameObject bot = botPool.Spawn(spawnPosition, Quaternion.identity , botPool.transform);
        // Set up the bot (e.g., adding it to a list, setting up callbacks, etc.)
    }

    // Call this method when a bot is destroyed/despawned
    public void RemoveBot(GameObject bot)
    {
        usedPositions.Remove(bot.transform.position);
        botPool.Despawn(bot);
    }
}