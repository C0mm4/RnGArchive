using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClose : CustomButton
{
    public Menu menu;
    public override void OnClick()
    {
        GameManager.UIManager.endMenu();
    }
}
