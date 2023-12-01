using TMPro;
using UnityEngine;

public class Player : Character, ICombatant
{
    [SerializeField] private GameObject floatingLevelTextPrefab;
    [SerializeField] private Transform canvasPopup;
    PlayerData playerData;
    private const string playerDataTxt = "playerData.txt";

    public override void Awake()
    {
        base.Awake();
        playerData = PlayerData.ReadFromJson(playerDataTxt) ?? new PlayerData();

        //playerData = PlayerData.ReadFromJson(playerDataTxt) ?? new PlayerData();
        //WeaponOnShop weaponOnShop = WeaponShopManagerItem.Instance.GetSelectWeapon();
        //if (!WeaponShopManagerItem.Instance.IsUnlockItem(WeaponType.HAMMER))
        //{
        //    WeaponShopManagerItem.Instance.BuyItem(WeaponType.HAMMER);
        //    WeaponShopManagerItem.Instance.SelectWeapon(WeaponType.HAMMER);
        //}
        //Weapon = weaponOnShop.prefabWeapon.GetComponent<Weapon>();
        //EquipWeapon(weaponOnShop);
        //InitWeaponOnHand();
        //BulletPrefab = weaponOnShop.prefabBullet;
    }
    public void GainCoin(int coin)
    {
        playerData.coin += coin;
    }
    public void ShowFloatingText(int level)
    {
        var floatText = Instantiate(floatingLevelTextPrefab, canvasPopup.position, Quaternion.identity, canvasPopup);
        floatText.GetComponent<TextMeshProUGUI>().text = $"+ {level}";
    }
    private void LateUpdate()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 newPos = CharacterModel.transform.position + Vector3.up * 6f;
        canvasPopup.position = newPos;
        canvasPopup.LookAt(newPos + cameraForward, Vector3.up);
    }
    //public void EquipWeapon(WeaponOnShop weapon)
    //{
    //    ApplyWeaponBonuses(weapon.bonusSpeed, weapon.bonusRange);
    //}
    public void ApplyWeaponBonuses(float bonusSpeed, float bonusRange)
    {
        this.Speed = BaseSpeed;
        this.Range = BaseRange;

        this.Speed += bonusSpeed / 10;
        this.Range += bonusRange / 10;
    }
    public string SerializeToJson()
    {
        PlayerData data = new PlayerData
        {
        };

        return JsonUtility.ToJson(data);
    }
}
