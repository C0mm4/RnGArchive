using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShot : PlayerState
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
        float animationSpd = player.body.velocity.x / player.charactor.charaData.maxSpeed;
        if(animationSpd > 0)
        {
            player.AnimationPlayLeg("Move", animationSpd);
        }
        else
        {
            player.AnimationPlayLeg("BackMove", -animationSpd);
        }

        base.UpdateState();
        if (Time.time - player.lastAttackT >= player.charactor.charaData.attackSpeed)
        {   
            player.charactor.EndState();
        }
        if (!player.isGrounded)
        {
            player.charactor.ChangeState(new JumpShot());
        } 
        else if (!player.isMove)
        {
            player.charactor.ChangeState(new Shot());
        }
    }
}
