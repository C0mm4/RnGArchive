using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class AIModel
{
    public Mob target;
    public GameObject player;

    protected string currentState;
    public abstract void Step();

    public virtual void DestroyHandler()
    {

    }

    public abstract void StateControl();
}
