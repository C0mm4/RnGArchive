using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Particle : Obj
{
    public Vector3 sPos;
    public Vector3 ePos;
    public float ableTime;
    float t;

    public Animator animator;
    public string currentAnimation;

    public override void OnCreate()
    {
        base.OnCreate();

    }

    public void CreateHandler(Vector3 startPos, Vector3 endPos, float time = 1f) 
    {
        t = 0;
        sPos = startPos; ePos = endPos;
        transform.position = sPos;
        ableTime = time;

        animator = GetComponent<Animator>();
        AnimationPlay("Play", 1);
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        t += Time.deltaTime;
        transform.position = Vector3.Lerp(sPos, ePos, t / ableTime);
        if (t >= ableTime)
        {
            Destroy();
        }
    }


    public void AnimationPlay(string clip, float spd)
    {
        if(animator != null)
        {
            if (!currentAnimation.Equals(clip))
            {
                if (System.Array.Exists(animator.runtimeAnimatorController.animationClips.ToArray(), findClip => findClip.name.Equals(clip)))
                {
                    var stateinfo = animator.GetCurrentAnimatorStateInfo(0);
                    float normalizeT = stateinfo.normalizedTime;
                    float animationLength = stateinfo.length;

                    currentAnimation = clip;
                    animator.speed = spd;
                    animator.Play(clip, 0, normalizeT / animationLength);
                }
                else
                {
                    Debug.Log($"Can't Find Clip {clip}");
                }
            }
            else
            {
                animator.speed = spd;
            }
        }
    }
}
