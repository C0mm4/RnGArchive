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
        if(player != null)
        {
            // Animate Idle Animation
            player.AnimationPlayBody("Idle");
            player.AnimationPlayLeg("Idle");



            // Translate State on player action
            if (!player.isGrounded)
            {
                player.charactor.ChangeState(new Jump());
            }
            else if (player.isMove)
            {
                player.charactor.ChangeState(new Move());
            }
            else if (player.isAttackInput && player.isAttack && !player.isAction)
            {
                player.charactor.ChangeState(new Shot());
            }

        }
        else if(charactor != null)
        {
            charactor.AnimationPlay("Idle");

            if (charactor.isMove)
            {
                charactor.ChangeState(new MobMove());
            }
        }
    }

}
