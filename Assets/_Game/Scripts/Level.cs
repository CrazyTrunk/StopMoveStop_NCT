using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int totalBotsToKill;
    //spawn pos start in level
    [SerializeField] private Vector2 spawnAreaMin;
    //spawn pos end in level
    [SerializeField] private Vector2 spawnAreaMax;

    public Vector2 SpawnAreaMax { get => spawnAreaMax; set => spawnAreaMax = value; }
    public Vector2 SpawnAreaMin { get => spawnAreaMin; set => spawnAreaMin = value; }
    public int TotalBotsToKill { get => totalBotsToKill; set => totalBotsToKill = value; }
}
