using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DMGUI : Obj
{
    public TMP_Text txt;

    public override void OnCreate()
    {
        base.OnCreate();
        SetAlarm(0, 1f);
    }

    public void SetDMGUI(int dmg)
    {
        txt.text = dmg.ToString();
    }


    public override void Alarm0()
    {
        base.Alarm0();
        Destroy();
    }
}
