using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobJumpFinish : State
{

    public override void UpdateState()
    {
        // Animate Idle Animation
        charactor.AnimationPlay("JumpFinish");

        if(charactor.velocity.y <= 0)
        {
            charactor.EndCurrentState();
        }
    }

    public override void ExitState()
    {
        charactor.ChangeState(new MobFalling());
        base.ExitState();
    }
}
