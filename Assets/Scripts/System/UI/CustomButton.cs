using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : HoveringRectTransform
{
    public Button button;


    public override void OnCreate()
    {
        base.OnCreate();
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


    }

    public virtual void OnClick()
    {

    }

    public override void pointerEnterEvent(PointerEventData eventData)
    {
        ExecuteEvents.Execute(button.gameObject, eventData, ExecuteEvents.pointerEnterHandler);
    }

    public override void pointerExitEvent(PointerEventData eventData)
    {
        ExecuteEvents.Execute(button.gameObject, eventData, ExecuteEvents.pointerExitHandler);
    }
}
