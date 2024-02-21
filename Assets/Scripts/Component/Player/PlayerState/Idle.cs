using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : PlayerState
{
    public override void EnterState(PlayerController chr)
    {
        base.EnterState(chr);
        chr.isMove = false;
    }

    public override void UpdateState()
    {
        // Animate Idle Animation
        player.AnimationPlayBody("Idle");
        player.AnimationPlayLeg("Idle");



        // Translate State on player action
        if (!player.IsGrounded)
        {
            player.charactor.ChangeState(new Jump());
        }
        else if (Mathf.Abs(player.velocity.x) != 0)
        {
            player.charactor.ChangeState(new Move());
        }
        else if (player.isAttackInput && player.isAttack && !player.isAction)
        {
            player.charactor.ChangeState(new Shot());
        }
    }

}
