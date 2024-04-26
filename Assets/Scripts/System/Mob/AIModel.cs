using System.Collections;
using UnityEngine;

public abstract class AIModel
{
    public Mob target;
    public PlayerController player;

    protected string currentState;
    public abstract void Step();

    public virtual void DestroyHandler()
    {

    }
}
