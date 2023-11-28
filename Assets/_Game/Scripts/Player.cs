using TMPro;
using UnityEngine;

public class Player : Character, ICombatant
{
    [SerializeField] private GameObject floatingLevelTextPrefab;
    [SerializeField] private Transform canvasPopup;
    public void ShowFloatingText(int level)
    {
        var floatText = Instantiate(floatingLevelTextPrefab, canvasPopup.position , Quaternion.identity, canvasPopup);
        floatText.GetComponent<TextMeshProUGUI>().text = $"+ {level}";
    }
    private void LateUpdate()
    {
        canvasPopup.LookAt(canvasPopup.position +
            Camera.main.transform.rotation * Vector3.forward, Vector3.up);
    }
}
