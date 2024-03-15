using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class State
{
    [SerializeField]
    protected Mob charactor;
    [SerializeField]
    protected PlayerController player;

    // State handers
    public virtual void EnterState(Mob chr)
    {
        charactor = chr;
    }

    public virtual void EnterState(PlayerController player)
    {
        this.player = player;
    }

    public virtual void ExitState() 
    {
        charactor = null;
    }
    

    public virtual void UpdateState()
    {

    }

}
