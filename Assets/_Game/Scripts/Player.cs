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

    private void Start()
    {
        PlayerData = GameManager.Instance.GetPlayerData();
        CharacterName = playerData.playerName;
    }
    void OnEnable()
    {
        GameManager.Instance.OnPlayerDataUpdated += UpdateWeapon;
        GlobalEvents.OnXMarkSkinShopClicked += ChangeSkin;
        GlobalEvents.OnXMarkSkinShopClicked += ResetIdle;
    }


    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDataUpdated -= UpdateWeapon;
        }
        GlobalEvents.OnXMarkSkinShopClicked -= ChangeSkin;
        GlobalEvents.OnXMarkSkinShopClicked -= ResetIdle;


    }
    private void ResetIdle()
    {
        ChangeAnim(Anim.IDLE);
    }
    private void UpdateWeapon(PlayerData data)
    {
        ChangeWeapon(data.equippedWeaponId);
        RecalculateBonuses();
        CharacterSphere.UpdateTriggerSize(Range);
    }
    public void GainCoin(int coins)
    {
        CoinGained += coins;
        PlayerData.coin += coins;
    }

    public void ShowFloatingText(int level)
    {
        var floatText = Instantiate(floatingLevelTextPrefab, canvasPopup.position, canvasPopup.rotation, canvasPopup);
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
