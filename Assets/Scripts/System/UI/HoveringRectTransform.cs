using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class HoveringRectTransform : Obj, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rect;
    public Vector2 position;
    public Vector2 size;

    public Menu menu;
    public int targetIndex;

    public override void OnCreate()
    {
        base.OnCreate();
        
        rect = GetComponent<RectTransform>();
        position = rect.position;
        size = rect.rect.size * 1.1f;

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
        position = rect.position;


        size = rect.rect.size * 1.1f;
    }

    public abstract void pointerEnterEvent(PointerEventData eventData);
    public abstract void pointerExitEvent(PointerEventData eventData);
}
