using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : InteractionTrigger
{
    public override void OnCreate()
    {
        base.OnCreate();

        detectDistance = GameManager.tileOffset.x * 2;
    }
}
