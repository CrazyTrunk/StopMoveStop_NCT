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