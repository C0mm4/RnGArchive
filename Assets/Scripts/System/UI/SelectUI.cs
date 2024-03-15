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
    int currentIndex;
    public int selectIndex;

    public override void OnCreate()
    {
        base.OnCreate();
        transform.SetParent(GameManager.UIManager.canvas.transform, false);
        selectIndex = -1;
    }

    public void CreateHandler(int size, List<string> txts)
    {
        listSize = size;
        currentIndex = 0;
        buttons = new List<SelectButton>();
        for(int i = 0; i < size; i++)
        {
            GameObject go = GameManager.InstantiateAsync("SelectButton");
            SelectButton sb = go.GetComponent<SelectButton>();
            sb.CreateHandler(i, txts[i]);
            sb.SetUI(this);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0, ((size - 1 - i) * 150), 0);
            buttons.Add(sb);
        }
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

    public override void KeyInput()
    {
        base.KeyInput();
        if(Input.GetKeyDown(GameManager.Input._keySettings.upKey))
        {
            currentIndex--;
            currentIndex %= listSize;
        }
        if (Input.GetKeyDown(GameManager.Input._keySettings.downKey))
        {
            currentIndex++;
            currentIndex %= listSize;
        }
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(buttons[currentIndex].button.gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);

        if (Input.GetKeyDown(GameManager.Input._keySettings.Interaction))
        {
            ConfirmAction();
        }
    }

    public override void ConfirmAction()
    {
        ExecuteEvents.Execute(buttons[currentIndex].button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }
}
