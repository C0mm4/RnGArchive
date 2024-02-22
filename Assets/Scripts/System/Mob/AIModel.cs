using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public abstract class AIModel
{
    public Mob target;
    public PlayerController player;
    public abstract void Step();

    public virtual void DestroyHandler()
    {

    }
}
