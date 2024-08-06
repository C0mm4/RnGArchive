using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public abstract class Menu : Obj
{
    [SerializeField]
    protected bool isGetInput;

    // Start is called before the first frame update
    public override void OnCreate()
    {
        GameManager.UIManager.addMenu(this);
        transform.SetParent(GameManager.UIManager.canvas.transform, false);
        isGetInput = false;
    }

    // when Menu hide, input deset, gameObject SetActive false
    public virtual void hide()
    {
        gameObject.SetActive(false);
    }
    // when Menu show, input Set, gameObject Active true
    public virtual void show()
    {
        gameObject.SetActive(true);
        GameManager.PauseGame();
        SetAlarm(0, 0.5f);

    }

    // when Menu Exit, Input Deset, GameObject Destroy
    public virtual async void exit()
    {
        AnimationPlay(GetComponent<Animator>(), "Close");
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        hide();
        GameManager.Input.MenuCloseT = Time.time;
        GameManager.ResumeGame();
        Destroy();
    }

    // some menus have confirm button. that buttons action
    public abstract void ConfirmAction();

    public override void KeyInput()
    {
        if (isGetInput)
        {
            // ESC == Exit Menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.UIManager.endMenu();
            }
            base.KeyInput();
        }
    }

    public override void Alarm0()
    {
        isGetInput = true;
    }
}
