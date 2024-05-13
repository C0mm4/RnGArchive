using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFinish : PlayerState
{

    public override void UpdateState()
    {
        // Animate Idle Animation
        player.AnimationPlayBody("JumpFinish");

        if(player.body.velocity.y <= 0)
        {
            player.EndCurrentState();
        }
    }

    public override void ExitState()
    {
        player.charactor.ChangeState(new Falling());
        base.ExitState();
    }
}
