

using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coin;
    public int levelMap;
    public bool isSoundOn;
    public bool isAdsRemove;
    public PlayerData()
    {
        
    }
    public void OnInitData()
    {
        coin = 0;
        levelMap = 0;
        isSoundOn = false;
        isAdsRemove = false;
        if (!WeaponData.IsUnlocked(WeaponType.HAMMER))
        {
            WeaponData.UnlockWeapon(WeaponType.HAMMER);
            WeaponData.SelectWeapon(WeaponType.HAMMER);
        }
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static PlayerData ReadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            //Debug.Log("File not found: " + filePath);
            return null;
        }

        try
        {
            string jsonString = File.ReadAllText(filePath);

            return JsonUtility.FromJson<PlayerData>(jsonString);
        }
        catch (Exception)
        {
            //Debug.Log("Failed to read from JSON file: " + e.Message);
            return null;
        }
    }
    public static void SaveToJson(PlayerData playerData, string filePath)
    {
        try
        {
            string jsonString = playerData.ToJson();
            File.WriteAllText(filePath, jsonString);
            Debug.Log("Data saved to: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save to JSON file: " + e.Message);
        }
    }
}
