using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove : PlayerState
{
    float startT;
    float releaseT;
    int index;

    public SpecialMove(float time, int index)
    {
        this.index = index;
        this.startT = Time.time;
        this.releaseT = time;
    }
    public override void EnterState(PlayerController chr)
    {
        base.EnterState(chr);
        player.isAction = true;
    }

    public override void ExitState()
    {
        player.isAction = false;
        base.ExitState();
    }

    public override void UpdateState()
    {
        if(Time.time - startT > releaseT)
        {
            player.charactor.EndState();
        }
        else
        {
            base.UpdateState();
            player.workingSkill.Step();
        }
    }
}
