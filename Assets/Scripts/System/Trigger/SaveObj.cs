using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObj : InteractionTrigger
{

    public override void OnCreate()
    {
        base.OnCreate();
        text = "�����Ѵ�";
    }
    public override void Interaction()
    {
        base.Interaction();
        GameManager.Save.SaveGameprogress(transform);
    }
}
