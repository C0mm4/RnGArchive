using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriggerManager
{
    public void Initialize()
    {

    }

    public void ActiveTrigger(TriggerData trig)
    {
        GameManager.Progress.activeTrigs[trig.id] = trig;

        trig.isActivate = true;

        Debug.Log(GameManager.Progress.activeTrigs.Count);
    }
}
