using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPrepareJump : State
{

    public override void UpdateState()
    {
        // Animate Idle Animation
        charactor.AnimationPlay("PrepareJump");

        
    }

    public override void ExitState()
    {
        charactor.ChangeState(new MobJumpFinish());
        base.ExitState();
    }
}
