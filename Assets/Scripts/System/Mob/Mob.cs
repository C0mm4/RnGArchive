using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.AudioSettings;

public class Mob : RigidBodyObject
{

    // Mob Attack Prefab
    [SerializeField]
    public GameObject[] attackPrefab;
    public bool[] isHitbox;
    public List<float> lastAttackT = new List<float>();
    public MobData data;


    public AIModel AI;

    [SerializeField]
    public SerializeStatus status;

    public StateMachine stateMachine;


    public string currentState;

    public GameObject attackObj;

    public Animator animator;
    public string currentAnimation;

    public bool isMove, isAttack;
    public bool isSet = false;

    public override void OnCreate()
    {
        base.OnCreate();
        animator = GetComponentInChildren<Animator>();
        transform.tag = "Enemy";
        stateMachine = new StateMachine(this);
        int layer = LayerMask.NameToLayer("Enemy");
        gameObject.layer = layer;
    }

    public virtual void CreateHPBar()
    {
        GetComponentInChildren<Trans2Canvas>().GenerateUI();
        GetComponentInChildren<Trans2Canvas>().UIObj.GetComponent<EnemyHPBar>().target = this;
    }

    public virtual void CreateHandler(Vector3 pos = default)
    {
        AIGen();
        
        status.maxHP = data.maxHP;
        status.currentHP = status.maxHP;
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>("SweaperAnimator");

        SetTargetPosition(pos);
        CreateHPBar();
        ChangeState(new Idle());
        isSet = true;
    }

    public void AIGen()
    {
        Debug.Log(data.AIModel);
        Type T = Type.GetType(data.AIModel);
        AI = Activator.CreateInstance(T) as AIModel;
        AI.target = this;
        AI.player = GameManager.player.GetComponent<PlayerController>();
    }

    public override void BeforeStep()
    {
        if (canMove)
        {
            var dir = targetMovePos - transform.position;
            if (dir.x < 0)
            {
                sawDir = new Vector2(-1, 0);
                body.velocity = new Vector2(-data.maxSpeed, body.velocity.y);
            }
            else
            {
                sawDir = new Vector2(1, 0);
                body.velocity = new Vector2(data.maxSpeed, body.velocity.y);
            }
        }
        else
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }

        base.BeforeStep();

        currentState = stateMachine.getStateStr();
    }

    public override void Step()
    {
        base.Step();
        if (AI != null)
        {
            AI.player = GameManager.player.GetComponent<PlayerController>();
            AI.Step();
        }
        stateMachine.updateState();
    }



    public void ChangeState(State newState)
    {
        stateMachine.changeState(newState);
    }

    public void EndState()
    {
        stateMachine.exitState();
    }

    public void SetIdle()
    {
        stateMachine.setIdle();
    }

    public override void FlipX()
    {
        if(sawDir.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void GetDMG(int dmg, AtkType type)
    {
        float multiplier = Func.GetDmgMultiplier(type, data.defType);
        int DMG = (int)(multiplier * dmg);
        GameObject DMGUITrans = GameManager.InstantiateAsync("DMGUI", SetDamageUIPos());
        DMGUITrans.GetComponent<Trans2Canvas>().GenerateUI();
        if (DMG == 0)
        {
            status.currentHP -= 1;
            DMGUITrans.GetComponent<Trans2Canvas>().UIObj.GetComponent<DMGUI>().txt.text = "1";
        }
        else
        {
            status.currentHP -= (int)(multiplier * dmg);
            DMGUITrans.GetComponent<Trans2Canvas>().UIObj.GetComponent<DMGUI>().txt.text = DMG.ToString();
        }
        if (status.currentHP < 0)
        {
            GameManager.Destroy(gameObject);
        }
    }

    public Vector3 SetDamageUIPos()
    {
        Vector3 ret = transform.position;

        ret += new Vector3(0, GetComponent<Collider2D>().bounds.extents.y * 2);

        return ret;
    }

    public void SetTargetPosition(Vector3 pos)
    {
        targetMovePos = pos;
    }

    public float GetPlayerDistance()
    {
        float ret;
        ret = Math.Abs(GameManager.player.transform.position.x - transform.position.x);
        ret -= GetComponent<Collider2D>().bounds.extents.x;
        return ret;
    }

    public void CreateAttackObj(int i)
    {
        string objName = gameObject.name.Replace("(Clone)", "").Trim() + " " + i.ToString();

        attackObj = GameManager.InstantiateAsync(objName);
        attackObj.transform.SetParent(transform);
        attackObj.transform.localPosition = Vector3.zero;

    }

    public void EndAttackState()
    {
        if (attackObj != null)
        {
            attackObj.GetComponent<MobAttackObj>().EndAttackState();
        }
    }

    public void DeleteAttackObj()
    {
        if (attackObj != null)
        {
            GameManager.Destroy(attackObj);
        }
    }

    public void AnimationPlay(string clip, float spd = 1f)
    {
        if (clip != currentAnimation)
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
