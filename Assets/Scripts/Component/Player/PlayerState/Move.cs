using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : PlayerState
{
    public Vector3 previousPos;

    public override void EnterState(PlayerController chr)
    {
        base.EnterState(chr);
        chr.isMove = true;
    }
    public override void UpdateState()
    {
        // Animate Idle Animation
        player.AnimationPlayBody("Move");
        player.AnimationPlayLeg("Move");


        // Translate State on player action
        if (!player.IsGrounded)
        {
            player.charactor.ChangeState(new Jump());
        }
        else if (player.isAttack)
        {
            player.charactor.ChangeState(new MoveShot());
        }

        previousPos = player.transform.position;
    }

}
