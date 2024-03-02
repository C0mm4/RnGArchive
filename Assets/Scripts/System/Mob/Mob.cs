using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Mob : KinematicObject
{
    // Mob Attack Prefab
    [SerializeField]
    public GameObject[] attackPrefab;
    public bool[] isHitbox;
    public List<float> lastAttackT = new List<float>();
    public MobData data;

    private Vector3 presentsawDir;

    public AIModel AI;

    public SerializeStatus status;

    public StateMachine stateMachine;

    

    public override void OnCreate()
    {
        base.OnCreate();
        animator = GetComponentInChildren<Animator>();
        transform.tag = "Enemy";
        stateMachine = new StateMachine(this);
    }

    public void CreateHandler(Vector3 pos = default)
    {
        AIGen();
        status.maxHP = data.maxHP;
        status.currentHP = status.maxHP;
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>("SweaperAnimator");

        SetTargetPosition(pos);
        ChangeState(new MobIdle());
    }

    public void AIGen()
    {
        Debug.Log(data.AIModel);
        Type T = Type.GetType(data.AIModel);
        AI = Activator.CreateInstance(T) as AIModel;
        AI.target = this;
        AI.player = GameManager.player.GetComponent<PlayerController>();
        Debug.Log(AI.GetType().Name);   
    }

    public override void BeforeStep()
    {
        if (canMove)
        {
            var dir = moveTargetPos - transform.position;
            if (dir.x < 0)
            {
                sawDir = new Vector2(-1, 0);
                moveAccel.x -= data.speedAccel * Time.deltaTime;
            }
            else
            {
                sawDir = new Vector2(1, 0);
                moveAccel.x += data.speedAccel * Time.deltaTime;
            }

            if (Mathf.Abs(moveAccel.x) > data.maxSpeed)
            {
                moveAccel.x = Vector2.Dot(sawDir, Vector2.one) * data.maxSpeed;
            }

            presentsawDir = dir;
        }
        else
        {
            moveAccel *= 0f;
        }

        base.BeforeStep();
    }

    public override void Step()
    {
        base.Step();
        if(AI != null)
        {
            AI.player = GameManager.player.GetComponent<PlayerController>();
            AI.Step();
        }
        else
        {
//            Debug.Log("AI is Null");
        }
    }

    protected override void ComputeVelocity()
    {
        targetVelocity.x = moveAccel.x;

    }

    public override void CheckCollision(RaycastHit2D rh, bool yMovement)
    {
        base.CheckCollision(rh, yMovement);

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

    // Attack Object Generate
    public void AttackObjGen(int index)
    {

    }

    public void GetDMG(int dmg, AtkType type)
    {
        float multiplier = Func.GetDmgMultiplier(type, data.defType);
        status.currentHP -= (int)(multiplier * dmg);
        Debug.Log(status.currentHP);
        if(status.currentHP < 0)
        {
            GameManager.Destroy(gameObject);
        }
    }

    public void SetTargetPosition(Vector3 pos)
    {
        moveTargetPos = pos;
    }

    public float GetPlayerDistance()
    {
        return (GameManager.player.transform.position - transform.position).magnitude;
    }
}
