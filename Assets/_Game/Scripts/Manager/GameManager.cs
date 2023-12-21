using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState _gameState;
    public event Action<PlayerData> OnPlayerDataUpdated;
    private PlayerData playerData;
    [SerializeField] private WeaponManagerDataScripableObject weaponDataSO;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        ChangeState(GameState.MENU);
        playerData = ReadFromJson(FilePathGame.CHARACTER_PATH);
        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.weapons = new List<int>
            {
                weaponDataSO.listWeapon.First().id
            };
            playerData.equippedWeaponId = weaponDataSO.listWeapon.First().id;
        }
        Cache.CachePlayerData(playerData);
        LevelManager.Instance.OnInit();
        ColorManager.Instance.OnInit();
    }
    public void ChangeState(GameState state)
    {
        _gameState = state;

    }
    public bool IsState(GameState gameState)
    {
        return _gameState == gameState;
    }
    public string ToJson(PlayerData playerData)
    {
        return JsonUtility.ToJson(playerData);
    }
    public PlayerData ReadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            string jsonString = File.ReadAllText(filePath);

            return JsonUtility.FromJson<PlayerData>(jsonString);
        }
        catch (Exception)
        {
            return null;
        }
    }
    public void SaveToJson(PlayerData playerData, string filePath)
    {
        try
        {
            string jsonString = ToJson(playerData);
            File.WriteAllText(filePath, jsonString);
            Debug.Log("Data saved to: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save to JSON file: " + e.Message);
        }
    }
    public void UpdatePlayerData(PlayerData newData)
    {
        playerData = newData;
        OnPlayerDataUpdated?.Invoke(playerData);
        Cache.UpdatePlayerData(playerData);
    }
    public PlayerData GetPlayerData()
    {
      return Cache.GetPlayerData();
    }

}