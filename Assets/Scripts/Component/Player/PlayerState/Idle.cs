using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : PlayerState
{
    public override void EnterState(PlayerController chr)
    {
        base.EnterState(chr);
        chr.isMove = false;

        Debug.Log("Idle");
    }

    public override void UpdateState()
    {
        if(player != null)
        {
            // Animate Idle Animation
            player.AnimationPlayBody("Idle");



            // Translate State on player action
            if (!player.isGrounded)
            {
                if(player.body.velocity.y > 0)
                {
                    player.charactor.ChangeState(new PrepareJump());
                }
                else
                {
                    player.charactor.ChangeState(new Falling());
                }
            }
            else if (player.isMove)
            {
                player.charactor.ChangeState(new Move());
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
