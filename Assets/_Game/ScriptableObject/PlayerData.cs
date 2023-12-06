

using System.Collections.Generic;
[System.Serializable]
public class PlayerData
{
    public int coin;
    public int levelMap = 1;
    public bool isSoundOn;
    public string playerName = "You";
    public bool isAdsRemove;
    public int equippedWeaponId;
    public List<int> weapons;
    public Dictionary<int, int> rankPerMap;
    public PlayerData()
    {
        rankPerMap = new Dictionary<int, int>();

    }

    public void UpdateHighestRankPerMap(int level, int rank)
    {
        if (rankPerMap.ContainsKey(level))
        {
            if (rank < rankPerMap[level])
            {
                rankPerMap[level] = rank;
            }
        }
        else
        {
            rankPerMap.Add(level, rank);
        }
    }
    public int GetHighestScoreByLevel(int level)
    {
        int score;
        if (rankPerMap.TryGetValue(level, out score))
        {
            return score;
        }
        else
        {
            return 0;
        }
    }
}
