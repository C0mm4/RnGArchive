using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing : PlayerState
{
    public override void EnterState(PlayerController controller)
    {
        base.EnterState(controller);
        player.isLanding = true;
        player.body.velocity = new Vector2(0, player.body.velocity.y);
    }

    public override void UpdateState()
    {
        // Animate Idle Animation
        player.AnimationPlayBody("Landing");

        player.canMove = false;
        player.isLanding = true;

    }

    public override void ExitState()
    {
        player.canMove = true;
        player.isLanding = false;
        if (player.isMove)
        {
            player.charactor.ChangeState(new Move());
        }
        else
        {
            player.charactor.ChangeState(new Idle());
        }
        base.ExitState();
    }
}
