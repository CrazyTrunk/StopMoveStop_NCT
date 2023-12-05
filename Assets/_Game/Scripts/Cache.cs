using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Cache
{
    private static PlayerData currentPlayerData;

    public static void CachePlayerData(PlayerData data)
    {
        currentPlayerData = data;
    }

    public static PlayerData GetPlayerData()
    {
        return currentPlayerData;
    }

    public static void UpdatePlayerData(PlayerData updatedData)
    {
        currentPlayerData = updatedData;
    }

}