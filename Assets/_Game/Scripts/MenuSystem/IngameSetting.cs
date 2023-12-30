public class IngameSetting : Menu<IngameSetting>
{
    public void OnContinueClick()
    {
        GameManager.Instance.ChangeState(GameState.PLAYING);
        IngameMenu.Instance.OnInit();
        IngameMenu.Instance.InitAliveText(LevelManager.Instance.TotalAlive);
    }
    public void OnHomeClick()
    {
        GameManager.Instance.ChangeState(GameState.MAIN_MENU);
        MainMenu.Instance.OnInit();
        LevelManager.Instance.OnInit();
    }
    private void OnDisable()
    {
        Hide();
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
