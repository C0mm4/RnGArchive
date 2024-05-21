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

            if (!charactor.isGrounded)
            {
                if (charactor.body.velocity.y > 0)
                {
                    charactor.ChangeState(new MobPrepareJump());
                }
                else
                {
                    charactor.ChangeState(new MobFalling());
                }
            }

            if (charactor.isMove)
            {
                charactor.ChangeState(new MobMove());
            }
        }
    }
}