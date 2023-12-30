using TMPro;
using UnityEngine;

public class IngameMenu : Menu<IngameMenu>
{
    [SerializeField]private TextMeshProUGUI alive;
    [SerializeField] private GameObject minimap;

    private PlayerData playerData;

    public void OnInit()
    {
        playerData = GameManager.Instance.GetPlayerData();
        if (playerData.isUsingMinimap)
        {
            minimap.SetActive(true);
        }
        else
        {
            minimap.SetActive(false);
        }
    }
    public void OnSettingClick()
    {
        Hide();
        IngameSetting.Show();
        IngameSetting.Instance.OnInit();

    }
    public void InitAliveText(int botAlive)
    {
        alive.text = $"Alive: {botAlive}";
    }
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
