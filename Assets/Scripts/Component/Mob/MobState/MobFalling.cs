using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobFalling : State
{

    public override void UpdateState()
    {
        // Animate Idle Animation
        charactor.AnimationPlay("Falling");

        if(charactor.isGrounded)
        {
            charactor.EndCurrentState();
        }
    }

    public override void ExitState()
    {
        charactor.ChangeState(new MobLanding());
        base.ExitState();
    }
}
