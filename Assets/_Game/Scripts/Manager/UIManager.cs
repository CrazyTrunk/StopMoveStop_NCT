
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MenuBase mainMenu;
    public MenuBase[] menuPrefabs;
    public static UIManager Instance { get; set; }
    private readonly Stack<MenuBase> menuStack = new Stack<MenuBase>();
    public Canvas canvasMenu;
    private void Awake()
    {
        Instance = this;

        //Instantiate and open default menu
        if (mainMenu != null)
        {
            var menu = Instantiate(mainMenu, canvasMenu.transform);
            OpenMenu(menu);
        }
    }
    public T CreateInstance<T>() where T : MenuBase
    {
        var prefab = GetPrefab<T>();

        return Instantiate(prefab, canvasMenu.transform);
    }
    private T GetPrefab<T>() where T : MenuBase
    {
        for (var i = 0; i < menuPrefabs.Length; i++)
        {
            if (menuPrefabs[i] != null && menuPrefabs[i].GetType() == typeof(T))
            {
                return (T)menuPrefabs[i];
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }
    public void OpenMenu(MenuBase instance)
    {
        // De-activate top menu


        if (menuStack.Count > 0)
        {
            if (instance.DisableMenusUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    menu.gameObject.SetActive(false);
                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }

        }

        //topCanvas.sortingOrder = maxSortingOrder + 1;
        if (menuStack.Count > 0)
        {
            var topMenu = menuStack.Peek();
            if (topMenu != null)
                topMenu.OnMenuBecameInvisible();


        }

        instance.OnMenuBecameVisible();
        if (!menuStack.Contains(instance))
            menuStack.Push(instance);
        //Debug.Log("OpenMenu: " + instance.name);
    }
    public void CloseMenu(MenuBase menu)
    {
        if (menuStack.Count == 0)
        {
            //Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
            return;
        }

        if (menuStack.Peek() != menu)
        {
            //Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
            return;
        }

        CloseTopMenu();
    }
    public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

        if (instance.DestroyWhenClosed)
            Destroy(instance.gameObject);
        else
            instance.gameObject.SetActive(false);

        // Re-activate top menu
        // If a re-activated menu is an overlay we need to activate the menu under it

        MenuBase topMenu = null;
        if (menuStack.Count > 0)
        {
            topMenu = menuStack.Peek();
        }


        if (topMenu != null)
        {
            foreach (var menu in menuStack)
            {
                menu.gameObject.SetActive(true);
                if (menu.DisableMenusUnderneath)
                    break;
            }

            topMenu.OnMenuBecameVisible();
        }
    }
}
