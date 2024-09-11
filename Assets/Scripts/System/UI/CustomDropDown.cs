using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CustomDropDown : HoveringRectTransform
{
    
    public TMP_Dropdown dropdown;

    public override void OnCreate()
    {
        base.OnCreate();
        try
        {
            dropdown = GetComponent<TMP_Dropdown>();

        }
        catch { }


    }

    public void OnClick()
    {

    }

    public override void pointerEnterEventOnCode(PointerEventData eventData)
    {
        ExecuteEvents.Execute(dropdown.gameObject, eventData, ExecuteEvents.pointerEnterHandler);
    }

    public override void pointerExitEventOnCode(PointerEventData eventData)
    {

        ExecuteEvents.Execute(dropdown.gameObject, eventData, ExecuteEvents.pointerExitHandler);
    }
}
