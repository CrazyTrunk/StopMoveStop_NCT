using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshUI;
    [SerializeField] private Character character;
    // Start is called before the first frame update
    void Awake()
    {
        textMeshUI.text = character.level.ToString();
    }
    public void UpdateUILevelPlayer(int level)
    {
        textMeshUI.text = level.ToString();
    }
}
