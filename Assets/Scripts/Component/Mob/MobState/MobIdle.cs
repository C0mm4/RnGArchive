using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobIdle : State
{
    public override void EnterState(Mob chr)
    {
        base.EnterState(chr);
        chr.isMove = false;

    }

    public override void UpdateState()
    {
        base.UpdateState();
        // if Target is Mob control mob state
        if (charactor != null)
        {
            charactor.AnimationPlay("Idle");

        }
    }
}
