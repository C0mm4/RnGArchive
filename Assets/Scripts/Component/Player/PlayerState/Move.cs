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
        float animationSpd = player.body.velocity.x * player.sawDir.x / player.charactor.charaData.maxSpeed;
        if (animationSpd > 0)
        {
            player.AnimationPlayLeg("Move", animationSpd);
        }
        else
        {
            player.AnimationPlayLeg("BackMove", -animationSpd);
        }

        // Translate State on player action
        if (!player.isGrounded)
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
