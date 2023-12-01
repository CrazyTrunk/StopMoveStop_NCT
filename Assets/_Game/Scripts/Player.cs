using TMPro;
using UnityEngine;

public class Player : Character, ICombatant
{
    [SerializeField] private GameObject floatingLevelTextPrefab;
    [SerializeField] private Transform canvasPopup;
    private void Start()
    {
        if (!WeaponShopManagerItem.Instance.IsUnlockItem(WeaponType.HAMMER))
        {
            WeaponShopManagerItem.Instance.BuyItem(WeaponType.HAMMER);
            WeaponShopManagerItem.Instance.SelectWeapon(WeaponType.HAMMER);
        }
        Weapon = WeaponShopManagerItem.Instance.GetSelectWeapon().prefabWeapon.GetComponent<Weapon>();
        InitWeaponOnHand();
        BulletPrefab = WeaponShopManagerItem.Instance.GetSelectWeapon().prefabBullet;
    }
    public void ShowFloatingText(int level)
    {
        var floatText = Instantiate(floatingLevelTextPrefab, canvasPopup.position , Quaternion.identity, canvasPopup);
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
