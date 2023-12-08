
using UnityEngine;

public class SkinMenu : Menu<SkinMenu>
{
    public void OnXMarkClick()
    {
        Hide();
        MainMenu.Show();    
        MainMenu.Instance.OnInit(); 
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.ResetCameraToOriginalPosition();
    }
    public void OnInit()
    {

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
