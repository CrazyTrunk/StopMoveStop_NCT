using System;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    public event Action OnPlayButtonPressed;

    public void OnPlayButtonClick()
    {
        Hide();
        GameManager.Instance.ChangeState(GameState.Playing);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.SwitchCameraViewToPlayer();
        OnPlayButtonPressed?.Invoke();
    }
    public void OnShopMenuClick()
    {
        Hide();
        WeaponMenu.Show();
        WeaponShopManagerItem.Instance.LoadFistItemInList();
    }
    public void OnMenuLevelSelected()
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
