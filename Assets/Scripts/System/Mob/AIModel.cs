using System.Collections;
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
