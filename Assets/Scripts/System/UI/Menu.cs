using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

[Serializable]
public abstract class Menu : Obj
{
    [SerializeField]
    protected bool isGetInput;

    [SerializeField]
    protected UIClose closeButton;

    public int cursorIndex;

    [SerializeField]
    protected GameObject hoveringUI;
    protected bool isHoveringAnimation = false;

    [SerializeField]
    private HoveringRectTransform hoveringTarget;

    [SerializeField]
    protected GameObject confirmButton;

    // Start is called before the first frame update
    public override void OnCreate()
    {
        GameManager.UIManager.addMenu(this);
        transform.SetParent(GameManager.UIManager.canvas.transform, false);
        isGetInput = false;

        hoveringUI = GameManager.InstantiateAsync("HoveringUI");
        hoveringUI.transform.SetParent(transform, false);
        hoveringUI.GetComponent<RectTransform>().localScale = Vector3.zero;

        var hoverableUIs = GetComponentsInChildren<HoveringRectTransform>(true);
        foreach(var obj in hoverableUIs)
        {
            obj.OnCreate();
        }
    }

    // when Menu hide, input deset, gameObject SetActive false
    public virtual void hide()
    {
        gameObject.SetActive(false);
    }
    // when Menu show, input Set, gameObject Active true
    public async virtual void show()
    {
        gameObject.SetActive(true);
        GameManager.PauseGame();
        
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        isGetInput = true;
    }

    // when Menu Exit, Input Deset, GameObject Destroy
    public virtual async void exit()
    {
        AnimationPlay(GetComponent<Animator>(), "Close");
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        hide();
        GameManager.Input.MenuCloseT = Time.time;
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


    public override void KeyInputAlways()
    {

    }

    public async virtual void OnMouseEnterHandler()
    {
        isHoveringAnimation = true;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        hoveringTarget = FindIndexButton(cursorIndex);

        hoveringTarget.pointerEnterEventOnCode(pointerEventData);

        if(hoveringUI != null)
        {
            hoveringUI.GetComponent<HoveringUI>().SetData(hoveringTarget.position, hoveringTarget.size);

            hoveringUI.GetComponent<RectTransform>().localScale = Vector3.zero;

            float t = 0f;
            while (t <= 0.1f)
            {
                t += Time.deltaTime;
                hoveringUI.GetComponent<RectTransform>().localScale = new Vector3(t * 10, t * 10, 1);
                await Task.Yield();
            }

            hoveringUI.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        isHoveringAnimation = false;
    }

    public override void StepAlways()
    {
        base.StepAlways();
        if(hoveringTarget != null && hoveringUI != null)
        {
            hoveringUI.GetComponent<HoveringUI>().SetData(hoveringTarget.position, hoveringTarget.size);
        }
    }

    public abstract HoveringRectTransform FindIndexButton(int index);
}
