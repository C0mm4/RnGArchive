using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriggerManager
{
    public Dictionary<string, TriggerData> activeTriggerLists;

    public void Initialize()
    {
        if(activeTriggerLists == null)
            activeTriggerLists = new Dictionary<string, TriggerData>();
        
    }

    public void ActiveTrigger(TriggerData trig)
    {
        if (activeTriggerLists == null)
            activeTriggerLists = new Dictionary<string, TriggerData>();
        activeTriggerLists[trig.id] = trig;

        trig.isActivate = true;
    }
}
