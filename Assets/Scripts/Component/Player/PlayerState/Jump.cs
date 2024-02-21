using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : PlayerState
{

    public override void UpdateState()
    {
        // Animate Idle Animation
        player.AnimationPlayBody("Jump");
        player.AnimationPlayLeg("Jump");


        // Translate State on player action
        if (player.IsGrounded)
        {
            player.charactor.SetIdle();
        }
        else if (player.isAttack)
        {
            player.charactor.ChangeState(new JumpShot());
        }
    }
}
