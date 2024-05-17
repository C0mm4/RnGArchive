using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTest : Obj
{
    public Animator animator;

    public bool isJump;

    public override void KeyInput()
    {
        base.KeyInput();

        if (Input.GetKeyDown(GameManager.Input._keySettings.Jump))
        {
            if (!isJump)
            {
                isJump = true;
                AnimationPlay("PrepareJump");
            }
        }

        if (Input.GetKey(GameManager.Input._keySettings.rightKey))
        {
            if (!isJump)
            {
                AnimationPlay("Move", 1f);
                transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
        }
        else if (Input.GetKey(GameManager.Input._keySettings.leftKey))
        {
            if (!isJump)
            {
                AnimationPlay("Move", 1f);
                transform.localRotation = new Quaternion(0, 180, 0, 0);
            }

        }
        else
        {
            AnimationPlay("Idle");
        }
    }

    public void AnimationPlay(string clip, float spd = 1)
    {
        if (!clip.Equals(currentAnimation))
        {
            if (System.Array.Exists(animator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
            {
                currentAnimation = clip;
                animator.speed = spd;
                animator.Play(clip);
            }
            else
            {
                Debug.Log($"Can't Find Clip : {clip}");
            }
        }
    }

    public void Landing()
    {
        isJump = false;
    }
}
