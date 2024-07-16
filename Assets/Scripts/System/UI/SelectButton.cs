using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButton : CustomButton
{
    SelectUI currentUI;
    public int index;
    [SerializeField]
    TMP_Text txt;

    public Image img;

    public override void OnCreate()
    {
        base.OnCreate();
        txt = GetComponentInChildren<TMP_Text>();
    }

    public async void CreateHandler(int i, string txt)
    {
        index = i;
        this.txt.text = txt;

        RectTransform rect = GetComponent<RectTransform>();
        float t = 0;
        while (t < 0.1f)
        {
            rect.localScale = new Vector3(t * 10, t * 10, 1);
            
            t += Time.deltaTime;
            await Task.Yield();
        }
        rect.localScale = new Vector3(1, 1, 1);

    }

    public void SetUI(SelectUI ui)
    {
        currentUI = ui;
    }

    public async override void OnClick()
    {
        if (!currentUI.isClick)
        {
            if (currentUI.hoveringIndex != -1)
            {
                currentUI.OnMouseClickHandler();
                img.color = Color.gray;
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                img.color = Color.white;
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                currentUI.selectIndex = index;
            }

        }
    }



    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!currentUI.isClick)
        {
            if (currentUI.hoveringIndex != index)
            {
                if (currentUI.hoveringIndex != -1)
                {
                    currentUI.hoveringIndex = index;
                    currentUI.OnMouseEnterHandler();
                }
            }

        }
    }

}
