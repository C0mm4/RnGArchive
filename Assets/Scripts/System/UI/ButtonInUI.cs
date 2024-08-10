using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInUI : CustomButton
{
    public Menu menu;
    public int targetIndex;

    public override void OnClick()
    {
        
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if(menu != null)
        {
            if(menu.cursorIndex != targetIndex)
            {
                if(menu.cursorIndex != -99)
                {
                    menu.cursorIndex = targetIndex;
                    menu.OnMouseEnterHandler();
                }
            }
        }
    }

}
