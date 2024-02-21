using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected KinematicObject charactor;
    protected PlayerController player;

    // State handers
    public virtual void EnterState(KinematicObject chr)
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
