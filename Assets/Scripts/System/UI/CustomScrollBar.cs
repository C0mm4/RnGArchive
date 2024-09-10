using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public override void pointerEnterEvent(PointerEventData eventData)
    {
        menu.OnMouseEnterHandler();
    }

    public override void pointerExitEvent(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
