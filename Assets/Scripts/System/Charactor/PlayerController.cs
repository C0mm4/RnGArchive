using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerController : RigidBodyObject
{
    public bool controlEnabled;


    public bool isHitState = false;
    public bool isImmune = false;

    public bool _isAction = false;
    public bool isAction { get { return _isAction; } set { _isAction = value; } }
    public bool isAttack = false;
    public bool _isLanding = false;
    public bool isLanding { get { return _isLanding;  } set { _isLanding = value; } }   
    public bool isInit = false;
    public bool isAttackInput = false;

    public Charactor charactor;
    public Supporter supporter;



    public List<InteractionTrigger> triggers;
    public int triggerIndex;

    public Animator animator;
    public Animator haloAnimator;
    public bool isSetAnimator;

    public Skill workingSkill;


    public string currentState;

    

    public override void OnCreate()
    {
        base.OnCreate();

        triggers = new();
        gravityModifier = 1f;

        sawDir = new Vector2(1, 0);
        
        charactor.charaData.currentSkin = 0;
        charactor.charaData.currentHalo = 0;

        int layer = LayerMask.NameToLayer("Player");
        gameObject.layer = layer;
    }

    public void CreateHandler()
    {
        charactor.playerController = this;

        gameObject.name = charactor.charaData.Name;

        charactor.stateMachine = new(this);

        charactor.ChangeState(new Idle());

        SetSkin(charactor.charaData.currentSkin);
        isInit = true;
    }


    public override void Step()
    {
        if (isInit && controlEnabled)
        {
            if (Mathf.Abs(body.velocity.x) <= 0.05)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }

            if (GameManager.GetUIState() == UIState.InPlay)
            {
                if(GameManager.Progress != null)
                {
                    // ActiveSkills, And Supporter Action
                    if (GameManager.Progress.isActiveSkill)
                    {
                        if (workingSkill != null)
                        {
                            workingSkill.Step();
                        }

                        if (charactor.passiveSkill.Count > 0)
                        {
                            foreach (Skill skill in charactor.passiveSkill)
                            {
                                skill.PassiveStep();
                            }
                        }
                    }
                }
                // Calculate Skill CoolTime
                foreach (Skill skill in charactor.skills)
                {
                    if (skill.isCool)
                    {
                        skill.leftCoolTime -= Time.deltaTime;
                        if (skill.leftCoolTime < 0)
                        {
                            skill.isCool = false;
                        }
                    }
                }

                // Cost Recovery
                if(charactor.charaData.currentCost < charactor.charaData.maxCost)
                {
                    float recoveryCost = charactor.charaData.costRecovery * Time.deltaTime;
                    charactor.charaData.currentCost += recoveryCost;
                    if (charactor.charaData.currentCost >= charactor.charaData.maxCost)
                    {
                        charactor.charaData.currentCost = charactor.charaData.maxCost;
                    }
                }


                // Interaction UI Generate
                if (triggers.Count > 0)
                {
                    GenInteractionUI();
                }

            }
            base.Step();


            currentState = charactor.stateMachine.getStateStr();

            if (!isAction && isGrounded && !isMove && !isLanding)
            {
                charactor.SetIdle();
            }

            if (charactor != null)
            {
                charactor.Step();
            }
        }
        else
        {
            if (isAction)
            {
                SetAlarm(2, 1f);
            }
        }
    }


    public override void KeyInput()
    {
        if((GameManager.GetUIState() == UIState.InPlay))
        {
            if (!isForceMoving)
            {
                base.KeyInput();
                if (!isHitState)
                {
                    if (canMove)
                    {
                        MoveKey();
                    }
                    else
                    {
                        body.velocity = new Vector2(0, body.velocity.y);
                    }
                    SkillKey();
                }
                if (Input.GetKeyDown(GameManager.Input._keySettings.Interaction))
                {
                    if(triggers.Count > 0)
                        triggers[triggerIndex].Interaction();
                    DeleteInteractionUI();
                }
            }
            else
            {
                Vector2 dir = new Vector2();
                if(targetMovePos.x > transform.position.x)
                {
                    dir.x = 1;
                }
                else
                {
                    dir.x = -1;
                }
                body.velocity = new Vector2(dir.x, body.velocity.y);
            }
        }
    }

    public void MoveKey()
    {
        if (Input.GetKey(GameManager.Input._keySettings.leftKey))
        {
            if (canMove)
            {
                body.velocity = new Vector2(-charactor.charaData.activeMaxSpeed, body.velocity.y);
                if (!isAction)
                {
                    sawDir = new Vector2(-1, sawDir.y);
                }
            }

        }
        else if (Input.GetKey(GameManager.Input._keySettings.rightKey))
        {
            if (canMove)
            {
                body.velocity = new Vector2(charactor.charaData.activeMaxSpeed, body.velocity.y);
                if (!isAction)
                {
                    sawDir = new Vector2(1, sawDir.y);
                }
            }

        }
        else
        {
            body.velocity = new Vector2(0f , body.velocity.y);
        }

        if (Input.GetKeyDown(GameManager.Input._keySettings.Jump) && isGrounded && canMove)
        {
            charactor.Jump();
            isGrounded = false;
        }

    }

    public void SkillKey()
    {
        if (Input.GetKeyDown(GameManager.Input._keySettings.Shot) && !isAttack)
        {
            charactor.Attack();
            SetAlarm(4, charactor.charaData.attackSpeed);
        }
        if(GameManager.Progress != null)
        {
            if (GameManager.Progress.isActiveSkill)
            {
                if (Input.GetKeyDown(GameManager.Input._keySettings.Skill1))
                {
                    if (!charactor.skills[0].isCool)
                        charactor.skills[0].Execute(sawDir);
                }

                if (Input.GetKeyDown(GameManager.Input._keySettings.Skill2))
                {
                    if (!charactor.skills[1].isCool)
                        charactor.skills[1].Execute(sawDir);
                }

                if (Input.GetKeyDown(GameManager.Input._keySettings.Skill3))
                {
                    if (!charactor.skills[0].isCool)
                        charactor.skills[0].Execute(sawDir);
                }
            }
        }
    }

    public override void FlipX()
    {
        if(sawDir.x > 0f)
        {
            transform.localRotation = new Quaternion(0, 0, 0, 0);

        }
        else
        {
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
        
    }



    public override void CheckCollision(Collision2D obj)
    {
        base.CheckCollision(obj);
        if (obj.gameObject.tag == "Enemy")
        {
            if (!isImmune)
            {
                GetDmg(obj.gameObject);
            }
        }
    }

    public void GetDmg(GameObject obj)
    {
        HPDecrease(1);
        body.AddForce((new Vector2((-obj.transform.position.x + transform.position.x) * 4, Vector2.up.y * 4)), ForceMode2D.Impulse);
        isImmune = true;
        isHitState = true;
        Physics2D.IgnoreLayerCollision(7,8, true);
        SetAlarm(0, 2f);
        SetAlarm(1, 1f);
    }

    public override void Alarm0()
    {
        isImmune = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    public override void Alarm1()
    {
        isHitState = false;
    }
    public override void Alarm2()
    {
        Addressables.ReleaseInstance(gameObject);
    }

    public override void Alarm3()
    {
        isAttackInput = false;
    }

    public override void Alarm4()
    {
        isAttack = false;
    }

    public override void Alarm5()
    {
        isAction = false;
    }



    public void SetSkin(int index)
    {
        charactor.charaData.currentSkin = index;

        animator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>
            (charactor.charaData.skins[charactor.charaData.currentSkin]);

        haloAnimator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>
            (charactor.charaData.haloSkins[charactor.charaData.currentHalo]);
        
        if(animator.runtimeAnimatorController != null)
        {
            isSetAnimator = true;
        }
    }

    public void AnimationPlayBody(string clip, float spd = 1)
    {
        if (isSetAnimator)
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
    }
/*
    public void AnimationPlayLeg(string clip, float spd = 1)
    {
        if (isSetLeg)
        {
             if (!clip.Equals(currentAnimationLeg))
             {
                if (System.Array.Exists(legAnimator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                {
                    var stateinfo = legAnimator.GetCurrentAnimatorStateInfo(0);
                    float normalizeT = stateinfo.normalizedTime;
                    float animationLength = stateinfo.length;

                    currentAnimationLeg = clip;
                    legAnimator.speed = spd;
                    legAnimator.Play(clip, 0, normalizeT / animationLength);
                }
                else
                {
                    Debug.Log($"Can't Find Clip : {clip}");
                }
            }
            else
            {
                legAnimator.speed = spd;
            }

        }
    }

    public void AnimationPlayHair(string clip, float spd = 1)
    {
        if (isSetBackHair)
        {
            if (!clip.Equals(currentAnimationLeg))
            {
                if (System.Array.Exists(backHairAnimator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                {
                    currentAnimationLeg = clip;
                    backHairAnimator.speed = spd;
                    backHairAnimator.Play(clip);
                }
                else
                {
                    Debug.Log($"Can't Find Clip : {clip}");
                }
            }
            else
            {
                backHairAnimator.speed = spd;
            }
        }
    }

    public void AnimationPlayHalo(string clip, float spd = 1)
    {
        if (isSetHalo)
        {
            if (!clip.Equals(currentAnimationLeg))
            {
                if (System.Array.Exists(haloAnimation.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                {
                    currentAnimationLeg = clip;
                    haloAnimation.speed = spd;
                    haloAnimation.Play(clip);
                }
                else
                {
                    Debug.Log($"Can't Find Clip : {clip}");
                }
            }
            else
            {
                haloAnimation.speed = spd;
            }
        }
    }*/

    public void AddInterractionTrigger(InteractionTrigger trigger)
    {
        if (!triggers.Contains(trigger))
        {
            triggers.Add(trigger);
        }
    }

    public void RemoveInteractionTrigger(InteractionTrigger trigger)
    {
        if (triggers.Contains(trigger))
        {
            triggers.Remove(trigger);
            if (triggerIndex >= triggers.Count)
            {
                triggerIndex = triggers.Count - 1;
            }
            if (triggerIndex < 0)
                triggerIndex = 0;
        }
    }

    public void GenInteractionUI()
    {
        GameManager.UIManager.GenerateInteractionUI();
    }

    public void DeleteInteractionUI()
    {
        GameManager.UIManager.DeleteInteractionUI();
    }

    public void HPIncrease(int value)
    {
        GameManager.CharaCon.charactors[charactor.charaData.id].charaData.currentHP += value;
        if (GameManager.CharaCon.charactors[charactor.charaData.id].charaData.currentHP > GameManager.CharaCon.charactors[charactor.charaData.id].charaData.maxHP)
        {
            GameManager.CharaCon.charactors[charactor.charaData.id].charaData.currentHP = GameManager.CharaCon.charactors[charactor.charaData.id].charaData.maxHP;
        }
    }

    public void HPDecrease(int value)
    {
        GameManager.Progress.charaDatas[charactor.charaData.id].charactor.charaData.currentHP -= value;
        if (GameManager.Progress.charaDatas[charactor.charaData.id].charactor.charaData.currentHP <= 0)
        {
            //            GameManager.ParticleGen("Particle_PlayerDie", transform.position, transform.position + new Vector3(0,10), 3);
            GameManager.PlayerDie();
        }
    }

    public void EndCurrentState()
    {
        charactor.EndState();
    }
}
