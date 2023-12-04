using System;
using TMPro;
using UnityEngine;

public class Player : Character, ICombatant
{
    [SerializeField] private GameObject floatingLevelTextPrefab;
    [SerializeField] private Transform canvasPopup;
    private int coinGained;
    private PlayerData playerData;

    public int CoinGained { get => coinGained; set => coinGained = value; }
    public PlayerData PlayerData { get => playerData; set => playerData = value; }

    public void LoadData(PlayerData playerData)
    {
        this.PlayerData = playerData;
    }
    private void OnEnable()
    {
        GlobalEvents.OnWeaponSelected += HandleWeaponSelection;
        GameManager.Instance.OnStateChanged += HandleStateChange;
    }
    private void OnDisable()
    {
        GlobalEvents.OnWeaponSelected -= HandleWeaponSelection;
        if(GameManager.Instance != null)
        GameManager.Instance.OnStateChanged -= HandleStateChange;
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
    private void HandleStateChange(GameState state)
    {
        if (GameManager.Instance.IsState(GameState.Playing))
        {
            LoadPlayerData();
        }
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
        PlayerData.coin += coins;
    }
    public void SaveGame()
    {
        PlayerData.SaveToJson(PlayerData, FilePathGame.CHARACTER_PATH);
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
