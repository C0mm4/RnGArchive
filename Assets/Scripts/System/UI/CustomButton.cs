using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Obj, IPointerEnterHandler, IPointerExitHandler
{

    public Button button;

    public Vector2 position;
    public Vector2 size;
    public Vector2 localPosition;
    public Vector2 anchoredPos;

    public Vector2 FuncPosition;

    public override void OnCreate()
    {
        try
        {
            button = GetComponent<Button>();
        }
        catch 
        {
            button = gameObject.AddComponent<Button>();
        }
        if(button == null)
        {
            button = gameObject.AddComponent<Button>();
        }
        button.onClick.AddListener(OnClick);


        position = GetComponent<RectTransform>().position;
        size = GetComponent<RectTransform>().rect.size * 1.1f;
    }

    public virtual void OnClick()
    {
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }
    public override void StepAlways()
    {
        base.StepAlways();
        position = GetComponent<RectTransform>().position;
        localPosition = GetComponent<RectTransform>().localPosition;
        anchoredPos = GetComponent<RectTransform>().anchoredPosition;

        FuncPosition = FindCanvasPosition();

        size = GetComponent<RectTransform>().rect.size * 1.1f;
    }

    public Vector2 FindCanvasPosition()
    {
        Vector3 ret = new(960, 540);

        List<RectTransform> rectList = new List<RectTransform>();
        Transform obj = transform.parent;
        while(!obj.name.Equals("Canvas"))
        {
            rectList.Add(obj.GetComponent<RectTransform>());
            obj = obj.parent;
        }
        var cnt = rectList.Count;
        for(int i = cnt -1; i >= 0; i--)
        {
            ret += new Vector3(ret.x * obj.GetComponent<RectTransform>().pivot.x, ret.y * obj.GetComponent<RectTransform>().pivot.y) + GetComponent<RectTransform>().localPosition;
        }

        return ret;
    }
}
