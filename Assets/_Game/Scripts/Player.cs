using System;
using TMPro;
using UnityEngine;

public class Player : Character, ICombatant
{
    [SerializeField] private GameObject floatingLevelTextPrefab;
    [SerializeField] private Transform canvasPopup;
    PlayerData playerData;
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
    }

    private void HandleWeaponSelection(WeaponType type)
    {
        ChangeWeapon(type);
        EquipWeapon(Weapon);
        CharacterSphere.UpdateTriggerSize(this.Range);
    }

    private void OnDisable()
    {
        GlobalEvents.OnWeaponSelected -= HandleWeaponSelection;
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


    public string SerializeToJson()
    {
        PlayerData data = new PlayerData
        {
        };

        return JsonUtility.ToJson(data);
    }
}
