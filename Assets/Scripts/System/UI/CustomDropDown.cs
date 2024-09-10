using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomDropDown : HoveringRectTransform
{
    
    public TMP_Dropdown dropdown;

    public override void OnCreate()
    {
        base.OnCreate();
        try
        {
            Debug.Log("dropdownSet");
            dropdown = GetComponent<TMP_Dropdown>();

        }
        catch { }


    }

    public void OnClick()
    {

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
