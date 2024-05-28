using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : PlayerState
{

    public override void UpdateState()
    {
        // Animate Idle Animation
        player.AnimationPlayBody("Falling");
        player.weapon.AnimationPlay(player.weapon.animator, "Falling");

        if(player.isGrounded)
        {
            player.EndCurrentState();
        }
    }

    public override void ExitState()
    {
        player.charactor.ChangeState(new Landing());
        base.ExitState();
    }
}
