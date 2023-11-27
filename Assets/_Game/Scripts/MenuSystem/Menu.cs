using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu<T> : MenuBase where T : Menu<T>
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = (T)this;
    }
    protected virtual void OnDestroy()
    {
        Instance = null;
    }

    protected static void Open()
    {
        if (Instance == null)
            UIManager.Instance.CreateInstance<T>();
        else
            Instance.gameObject.SetActive(true);

        UIManager.Instance.OpenMenu(Instance);
    }
    protected static void Close()
    {
        if (Instance == null)
        {
            //#if UNITY_EDITOR
            //            Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", typeof(T));
            //#endif
            return;
        }

        UIManager.Instance.CloseMenu(Instance);

    }
    public override void OnBackPressed()
    {
        if (!Cancelable)
            return;
        Close();
    }

    public override void OnMenuBecameInvisible()
    {
    }

    public override void OnMenuBecameVisible()
    {
    }
}
