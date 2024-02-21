using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UIManager
{
    static UIManager ui_instance;

    public Menu currentMenu = null;
    public Stack<Menu> menuStack = new Stack<Menu>();
    

    Stack<UIState> uistack = new Stack<UIState>();
    private UIState uiState;

    private bool isMapToggle = false;
    private bool isMapToggleActivate = false;

    public GameObject canvas;

    public static UIState UI { get { return ui_instance.uiState; } }

    public enum UIState
    {
        Loading, InPlay, Title, CutScene, Menu, Pause, 
    };

    public void initialize()
    {
        ui_instance = this;
        ChangeState(UIState.Title);
        GameObject go = GameObject.Find("Canvas");
        if (go != null)
        {
            canvas = go;
        }
        
    }

    // Generate Menu Object, add stacks and hide before menu and showing new menu
    public void addMenu(Menu menu)
    {
        if (currentMenu != null)
        {
            currentMenu.hide();
            menuStack.Push(currentMenu);
        }
        currentMenu = menu;
        ChangeStateOnStack(UIState.Menu);
        currentMenu.show();
    }

    // Menu object closed, reset before menu
    public void endMenu()
    {
        currentMenu.exit();
        currentMenu = null;
        if (menuStack.Count > 0)
        {
            currentMenu = menuStack.Pop();
            currentMenu.show();
        }
        PopStateStack();
    }

    // Change Ui State on Stack (Used Menus)
    public void ChangeStateOnStack(UIState state)
    {
        uistack.Push(uiState);
        ChangeState(state);
    }

    // When Menu Close Stack UI Change
    public void PopStateStack()
    {
        ChangeState(uistack.Pop());
    }

    // Change UI State
    public void ChangeState(UIState state)
    {

        uiState = state;

//        GameManager.Input.changeInputState();

    }

    public UIState GetUIState()
    {
        return uiState;
    }

    public void MapToggle()
    {
        if (!isMapToggleActivate)
        {
            if (isMapToggle)
            {
                isMapToggle = false;
                endMenu();
            }
            else
            {
                Addressables.InstantiateAsync("MiniMap").Completed += handle => 
                {
                    GameObject go = handle.Result;
                    go.GetComponent<Obj>().handle = handle;
                };
                isMapToggle = true;
            }

            isMapToggleActivate = true;

            Task.Run(async () =>
            {
                await Task.Delay(500);
                isMapToggleActivate = false;
            });
        }
    }

    public void Update()
    {
        if(canvas == null)
        {
            GameObject go = GameObject.Find("Canvas");
            if (go != null)
            {
                canvas = go;
            }
        }
    }
}
