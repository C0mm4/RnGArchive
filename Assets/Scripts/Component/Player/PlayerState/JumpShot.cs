using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpShot : PlayerState
{
    public override void EnterState(PlayerController chr)
    {
        base.EnterState(chr);
    }

    public override void ExitState()
    {
        player.isAction = false;
        base.ExitState();
    }

    public override void UpdateState()
    {
        player.AnimationPlayBody("Move");
        player.AnimationPlayLeg("Jump");


        base.UpdateState();
        if (Time.time - player.lastAttackT >= player.charactor.status.attackSpeed)
        {
            player.charactor.EndState();
        }

        if (player.IsGrounded)
        {
            player.charactor.ChangeState(new Shot());
        }
    }
}
