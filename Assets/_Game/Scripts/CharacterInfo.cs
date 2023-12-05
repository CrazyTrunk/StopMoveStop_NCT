using TMPro;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private Transform infoCanvasTransform;
    [SerializeField] private Transform characterModel;
    [SerializeField] private float yOffset = 3.0f;
    [SerializeField] TextMeshProUGUI levelUI;
    [SerializeField] TextMeshProUGUI nameUI;

    [SerializeField] private Character character;
    void Start()
    {
        levelUI.text = character.level.ToString();
        nameUI.text = character.CharacterName;
    }
    public void UpdateUILevelPlayer(int level)
    {
        levelUI.text = level.ToString();
    }
    public void UpdateUINamePlayer(string name)
    {
        nameUI.text = name;
    }
    private void LateUpdate()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        float scaledYOffset = yOffset + characterModel.transform.localScale.y;

        Vector3 newPos = characterModel.transform.position + Vector3.up * scaledYOffset;
        infoCanvasTransform.position = newPos;
        infoCanvasTransform.LookAt(newPos + cameraForward, Vector3.up);
    }
}
