using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : Obj
{
    [SerializeField]
    TMP_Text txt;
    [SerializeField]
    Image img;
    [SerializeField]
    RectTransform rt;

    public float t;

    public int state = 0;

    public override void OnCreate()
    {
        base.OnCreate();
        Func.SetRectTransform(gameObject);
    }

    public void SetText(string text)
    {
        txt.text = text;
    }

}
