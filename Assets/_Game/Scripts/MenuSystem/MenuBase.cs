using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuBase : MonoBehaviour
{
    [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
    public bool DestroyWhenClosed = true;

    [Tooltip("Disable menus that are under this one in the stack")]
    public bool DisableMenusUnderneath = true;

    [Tooltip("Cancelable menu when press back key")]
    public bool Cancelable = true;
    public abstract void OnMenuBecameVisible();
    public abstract void OnMenuBecameInvisible();
    public abstract void OnBackPressed();
}
