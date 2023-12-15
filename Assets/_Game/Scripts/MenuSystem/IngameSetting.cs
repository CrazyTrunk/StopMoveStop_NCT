using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameSetting : Menu<IngameSetting>
{
    private void Start()
    {
        GameManager.Instance.ChangeState(GameState.MENU);
    }
    public void OnContinueClick()
    {
        Debug.Log("Continue");
        Hide();
        GameManager.Instance.ChangeState(GameState.PLAYING);
        IngameMenu.Show();
        IngameMenu.Instance.OnInit(LevelManager.Instance.TotalAlive);
    }
    public void OnHomeClick()
    {
        Debug.Log("Load Lai Scene");
        Hide();
        MainMenu.Show();
        MainMenu.Instance.OnInit();
        LevelManager.Instance.OnInit();
        GameManager.Instance.ChangeState(GameState.MENU);
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
