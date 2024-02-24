using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mob : KinematicObject
{
    // Mob Attack Prefab
    [SerializeField]
    public GameObject[] attackPrefab;
    public bool[] isHitbox;
    public List<float> lastAttackT = new List<float>();
    public MobData data;

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

    public void CreateHandler()
    {
        AIGen();
        status.maxHP = data.maxHP;
        status.currentHP = status.maxHP;
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>("SweaperAnimator");
        ChangeState(new MobIdle());
    }

    public void AIGen()
    {
        Type T = Type.GetType(data.AIModel);
        AI = Activator.CreateInstance(T) as AIModel;
    }

    public override void Step()
    {
        base.Step();
        AI.Step();
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


}
