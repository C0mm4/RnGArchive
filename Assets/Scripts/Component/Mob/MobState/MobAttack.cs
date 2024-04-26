using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : State
{
    int attackIndex;
    float t;
    float animationTime;

    public MobAttack(int index)
    {
        attackIndex = index;
    }

    public override void EnterState(Mob chr)
    {
        base.EnterState(chr);
        charactor.body.velocity = new Vector2(0, charactor.body.velocity.y);
        charactor.isAttack = true;
        charactor.AnimationPlay("Attack " + attackIndex);
        var clip = getAnimationClip("Attack " + attackIndex);
        animationTime = clip.length;

        charactor.canMove = false;

        charactor.data.attackCooltime[attackIndex] = true;
        t = 0;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        t += Time.deltaTime;

        if(t >= animationTime)
        {
            charactor.stateMachine.exitState();
        }
        
    }

    public override void ExitState()
    {
        charactor.canMove = true;
        charactor.EndAttackState();
        charactor.isAttack = false;
        charactor.SetAlarm(attackIndex, charactor.data.attackDelay[attackIndex]);
        base.ExitState();
    }

    private AnimationClip getAnimationClip(string target)
    {
        var controller = charactor.animator.runtimeAnimatorController;
        foreach (var clip in controller.animationClips)
        {
            if(clip.name == target)
            {
                return clip;
            }
        }
        return null;
    }

}
