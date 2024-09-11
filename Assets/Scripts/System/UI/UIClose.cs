using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClose : CustomButton
{
    public override void OnCreate()
    {
        base.OnCreate();
        targetIndex = -1;
    }
    public override void OnClick()
    {
        if(GameManager.UIManager.currentMenu == menu)
        {
            GameManager.UIManager.endMenu();
        }
    }
}
