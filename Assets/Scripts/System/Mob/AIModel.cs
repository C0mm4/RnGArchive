using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AIModel
{
    public Mob target;

    public bool canAccessPlayer;

    public List<Vector2Int> path = new();

    protected string currentState;
    public abstract void Step();

    public virtual void DestroyHandler()
    {

    }

    public abstract void StateControl();
}
