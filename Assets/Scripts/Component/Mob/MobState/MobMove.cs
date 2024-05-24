using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMove : State
{
    public override void EnterState(Mob chr)
    {
        base.EnterState(chr);
        charactor.isMove = true;
        charactor.AnimationPlay("Move");
    }

    public override void ExitState()
    {
        charactor.isMove = false;
        base.ExitState();

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!charactor.isMove)
        {
            charactor.ChangeState(new MobIdle());
        }
    }

}
