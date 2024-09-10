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
        charactor.velocity = new Vector2(0, charactor.velocity.y);
        charactor.isAttack = true;
        charactor.AnimationPlay("Attack " + attackIndex);
        var clip = getAnimationClip("Attack " + attackIndex);
        animationTime = clip.length;

        charactor.canMove = false;

        charactor.data.attackIsCool[attackIndex] = true;
        t = 0;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        t += Time.deltaTime;

        if(t >= animationTime)
        {
            charactor.EndCurrentState();
        }
        
    }

    public override void ExitState()
    {
        charactor.canMove = true;
        charactor.isAttack = false;
        charactor.SetAlarm(attackIndex, charactor.data.attackDelay[attackIndex]);
        charactor.EndAttackState();
        charactor.SetIdle();
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
