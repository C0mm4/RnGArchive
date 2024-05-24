using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPrepareAttack : State
{
    int attackIndex;
    float t;
    float animationTime;

    public MobPrepareAttack(int index)
    {
        attackIndex = index;
    }

    public override void EnterState(Mob chr)
    {
        base.EnterState(chr);
        Debug.Log("Mob Attack");
        charactor.velocity = new Vector2(0, charactor.velocity.y);
        charactor.isAttack = true;
        charactor.AnimationPlay("PrepareAttack " + attackIndex);
        var clip = getAnimationClip("PrepareAttack " + attackIndex);
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
        charactor.ChangeState(new MobAttack(attackIndex));

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
