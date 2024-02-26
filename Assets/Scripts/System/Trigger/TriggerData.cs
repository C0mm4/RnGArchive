using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriggerData 
{
    public string id;
    public bool isActivate;

    public TriggerData(string id, bool isActive)
    {
        this.id = id;
        this.isActivate = isActive;
    }
}
