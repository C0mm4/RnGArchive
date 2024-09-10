using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUI : Menu
{
    [SerializeField]
    GameObject ButtonObj;

    List<SelectButton> buttons;

    int listSize;
    public int selectIndex;


    public bool isClick = false;

    public override void OnCreate()
    {
        base.OnCreate();
        transform.SetParent(GameManager.UIManager.canvas.transform, false);
        selectIndex = -1;
        cursorIndex = -1;
    }

    public async void CreateHandler(int size, List<string> txts)
    {
        listSize = size;
        buttons = new List<SelectButton>();
        hoveringUI = GameManager.InstantiateAsync("HoveringUI");
        hoveringUI.transform.SetParent(transform, false);
        hoveringUI.GetComponent<RectTransform>().localScale = Vector3.zero;
        for (int i = 0; i < size; i++)
        {
            GameObject go = GameManager.InstantiateAsync("SelectButton");
            SelectButton sb = go.GetComponent<SelectButton>();
            sb.CreateHandler(i, txts[i]);
            sb.SetUI(this);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0, ((size - 1 - i) * 150), 0);
            buttons.Add(sb);
        }

        await Task.Delay(TimeSpan.FromMilliseconds(300));
        cursorIndex = 0;
        OnMouseEnterHandler();
    }

    public async Task<int> Select()
    {
        while(selectIndex == -1)
        {
            await Task.Yield();
        }
        GameManager.UIManager.endMenu();
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        return selectIndex;
    }

    public override void KeyInputAlways()
    {
        base.KeyInputAlways();

        if (isGetInput && !isHoveringAnimation)
        {
            if (!isClick)
            {
                if (listSize > 1)
                {
                    if (Input.GetKeyDown(GameManager.Input._keySettings.upKey))
                    {
                        cursorIndex--;
                        cursorIndex %= listSize;
                        OnMouseEnterHandler();
                        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                        if (cursorIndex != -1)
                            ExecuteEvents.Execute(buttons[cursorIndex].button.gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);

                    }
                    if (Input.GetKeyDown(GameManager.Input._keySettings.downKey))
                    {
                        cursorIndex++;
                        cursorIndex %= listSize;
                        OnMouseEnterHandler();
                        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                        if (cursorIndex != -1)
                            ExecuteEvents.Execute(buttons[cursorIndex].button.gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);

                    }
                }

                if (Input.GetKeyDown(GameManager.Input._keySettings.Interaction))
                {
                    ConfirmAction();
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ConfirmAction()
    {
        ExecuteEvents.Execute(buttons[cursorIndex].button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    public async override void OnMouseEnterHandler()
    {
        isHoveringAnimation = true;
        hoveringUI.GetComponent<HoveringUI>().SetData(buttons[cursorIndex].GetComponent<RectTransform>().position, new Vector2(1650, 150));
        hoveringUI.GetComponent<RectTransform>().localScale = Vector3.zero;

        float t = 0f;
        while(t <= 0.1f)
        {
            t += Time.deltaTime;
            hoveringUI.GetComponent<RectTransform>().localScale = new Vector3(t * 10, t * 10, 1);
            await Task.Yield();
        }
        hoveringUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        isHoveringAnimation = false;
    }

    public void OnMouseExitHandler()
    {
        
    }

    public void OnMouseClickHandler()
    {
        isClick = true;
        GameManager.Destroy(hoveringUI);
        cursorIndex = -1;

    }

    public override HoveringRectTransform FindIndexButton(int index)
    {
        return buttons[index];
    }
}
