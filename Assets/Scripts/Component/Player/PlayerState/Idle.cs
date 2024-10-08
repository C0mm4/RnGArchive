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
        // If Target is Player control player state
        if(player != null)
        {
            // Animate Idle Animation
            player.AnimationPlayBody("Idle");
            if (!player.weapon.currentAnimation.Equals("Fire") && !player.weapon.currentAnimation.Equals("Landing"))
            {

                player.weapon.AnimationPlay(player.weapon.animator, "Idle");
            }


            // Translate State on player action
            if (!player.isGrounded)
            {
                if(player.velocity.y > 0)
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
    }

}
