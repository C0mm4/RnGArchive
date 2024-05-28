using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareJump : PlayerState
{

    public override void UpdateState()
    {
        // Animate Idle Animation
        player.AnimationPlayBody("PrepareJump");
        player.weapon.AnimationPlay(player.weapon.animator, "PrepareJump");
        
    }

    public override void ExitState()
    {
        player.charactor.ChangeState(new JumpFinish());
        base.ExitState();
    }
}
