using System;
using TMPro;
using UnityEngine;

public class Player : Character, ICombatant
{
    [SerializeField] private GameObject floatingLevelTextPrefab;
    [SerializeField] private Transform canvasPopup;
    private int coinGained;
    PlayerData playerData;

    public int CoinGained { get => coinGained; set => coinGained = value; }

    public override void Awake()
    {
        base.Awake();
        ChangeWeapon(WeaponDataSO.CurrentEquipWeapon().type);
        EquipWeapon(Weapon);
        CharacterSphere.UpdateTriggerSize(this.Range);
    }
    public void LoadData(PlayerData playerData)
    {
        this.playerData = playerData;
    }
    private void OnEnable()
    {
        GlobalEvents.OnWeaponSelected += HandleWeaponSelection;
        GlobalEvents.OnPlayClick += LoadPlayerData;
    }

    private void LoadPlayerData()
    {
        playerData = PlayerData.ReadFromJson(FilePathGame.CHARACTER_PATH);
        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.OnInitData();
        }
    }

    private void OnDisable()
    {
        GlobalEvents.OnWeaponSelected -= HandleWeaponSelection;
        GlobalEvents.OnPlayClick -= LoadPlayerData;
    }
    private void HandleWeaponSelection(WeaponType type)
    {
        ChangeWeapon(type);
        EquipWeapon(Weapon);
        CharacterSphere.UpdateTriggerSize(this.Range);
    }
    public void GainCoin(int coins)
    {
        CoinGained += coins;
        playerData.coin += coins;
    }
    public void SaveGame()
    {
        PlayerData.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
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
}
