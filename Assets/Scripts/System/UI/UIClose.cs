using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClose : ButtonInUI
{
    public override void OnCreate()
    {
        base.OnCreate();
        targetIndex = -1;
    }
    public override void OnClick()
    {
        GameManager.UIManager.endMenu();
    }
}
