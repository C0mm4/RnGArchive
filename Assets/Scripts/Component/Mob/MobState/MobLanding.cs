using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobLanding : State
{
    public override void EnterState(Mob controller)
    {
        base.EnterState(controller);
        charactor.isLanding = true;
        charactor.body.velocity = new Vector2(0, charactor.body.velocity.y);
    }

    public override void UpdateState()
    {
        // Animate Idle Animation
        charactor.AnimationPlay("Landing");

        charactor.canMove = false;
        charactor.isLanding = true;

    }

    public override void ExitState()
    {
        charactor.canMove = true;
        charactor.isLanding = false;
        if (charactor.isMove)
        {
            charactor.ChangeState(new MobMove());
        }
        else
        {
            charactor.ChangeState(new MobIdle());
        }
        base.ExitState();
    }
}
