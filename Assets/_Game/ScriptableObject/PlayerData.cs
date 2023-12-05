

using System.Collections.Generic;
[System.Serializable]
public class PlayerData
{
    public int coin;
    public int levelMap = 1;
    public bool isSoundOn;
    public string playerName = "You";
    public bool isAdsRemove;
    public Weapon equippedWeapon;
    public List<Weapon> weapons;
}
