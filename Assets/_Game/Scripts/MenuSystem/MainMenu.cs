using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    public void OnPlayButtonClick()
    {
        Hide();
        GameManager.Instance.ChangeState(GameState.Playing);
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.SwitchCameraViewToPlayer();
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
