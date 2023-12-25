

using System.Collections.Generic;
[System.Serializable]
public class PlayerData
{
    public int coin;
    public int levelMap = 1;
    public bool isSoundOn;
    public bool isVibrance;
    public bool isUsingMinimap;
    public string playerName = "You";
    public bool isAdsRemove;
    public int equippedWeaponId;
    public int equippedSkinId;
    public List<int> weapons;
    public List<int> skins;

    public List<LevelDataRank> levelDataList;
    public PlayerData()
    {
        coin = 0;
        levelMap = 1;
        isSoundOn = true;
        isAdsRemove = false;
        isVibrance = true;
        isUsingMinimap = true;
        equippedWeaponId = -1;
        equippedSkinId = -1;
        levelDataList = new List<LevelDataRank>();
        skins = new List<int>();
        weapons = new List<int>();
    }

    public void UpdateHighestRankPerMap(int level, int rank)
    {
        foreach (var data in levelDataList)
        {
            if (data.level == level)
            {
                if (rank < data.highestRank)
                {
                    data.highestRank = rank;
                    return;
                }
                else
                {
                    return;
                }
            }
        }
        levelDataList.Add(new LevelDataRank(level, rank));
    }
    public int GetHighestScoreByLevel(int level)
    {
        foreach (var data in levelDataList)
        {
            if (data.level == level)
            {
                return data.highestRank;
            }
        }
        return 0;
    }
}
[System.Serializable]
public class LevelDataRank
{
    public int level;
    public int highestRank;

    public LevelDataRank(int level, int highestRank)
    {
        this.level = level;
        this.highestRank = highestRank;
    }
}