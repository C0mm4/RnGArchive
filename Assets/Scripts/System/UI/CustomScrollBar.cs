using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CustomScrollBar : HoveringRectTransform
{
    public Scrollbar scrollbar;

    public override void OnCreate()
    {
        base.OnCreate();
        try
        {
            scrollbar = GetComponent<Scrollbar>();
        }
        catch { }

        
    }

    public override void pointerEnterEventOnCode(PointerEventData eventData)
    {

        ExecuteEvents.Execute(scrollbar.gameObject, eventData, ExecuteEvents.pointerEnterHandler);
    }

    public override void pointerExitEventOnCode(PointerEventData eventData)
    {

        ExecuteEvents.Execute(scrollbar.gameObject, eventData, ExecuteEvents.pointerExitHandler);
    }
}
