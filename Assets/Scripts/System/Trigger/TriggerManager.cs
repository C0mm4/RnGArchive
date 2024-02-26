using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager
{
    public Dictionary<string, Trigger> activeTriggerLists;

    public void Initialize()
    {
        activeTriggerLists = new Dictionary<string, Trigger>();
        
    }

    public void ActiveTrigger(Trigger trig)
    {
        activeTriggerLists.Add(trig.id, trig);
        trig.isActivate = true;
    }
}
