using TMPro;
using UnityEngine;

public class IngameMenu : Menu<IngameMenu>
{
    [SerializeField]private TextMeshProUGUI alive;
    public void OnSettingClick()
    {
        Hide();
        IngameSetting.Show();
    }
    public void OnInit(int botAlive)
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
