using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectButton : CustomButton
{
    SelectUI currentUI;
    public int index;
    [SerializeField]
    TMP_Text txt;

    public override void OnCreate()
    {
        base.OnCreate();
        txt = GetComponentInChildren<TMP_Text>();
    }

    public void CreateHandler(int i, string txt)
    {
        index = i;
        this.txt.text = txt;
    }

    public void SetUI(SelectUI ui)
    {
        currentUI = ui;
    }

    public override void OnClick()
    {
        currentUI.selectIndex = index;
    }
}
