using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class Charactor
{
    public Dictionary<List<KeyValues>, Skill> commands;
    [SerializeField]
    public List<Skill> passiveSkill;

    public List<Skill> skills;

    public Animator bodyAnimator;
    public Animator legAnimator;
    public Animator haloAnimator;

    public StateMachine stateMachine;

    public CharactorData charaData;

    public PlayerController playerController;

    public string attackPref;

    public AtkType atkType;
    public DefType defType;

    public void SetType(string atkT, string defT)
    {
        atkT = atkT.ToLower();
        defT = defT.ToLower();
        switch (atkT)
        {
            case "explosive":
                atkType = AtkType.Explosive;
                break;
            case "piercing":
                atkType = AtkType.Piercing;
                break;
            case "mystic":
                atkType = AtkType.Mystic;
                break;
            case "sonic":
                atkType = AtkType.Sonic;
                break;
            case "normal":
                atkType = AtkType.Normal;
                break;
        }
        switch (defT)
        {
            case "normal":
                defType = DefType.Normal;
                break;
            case "light":
                defType = DefType.Light;
                break;
            case "heavy":
                defType = DefType.Heavy;
                break;
            case "special":
                defType = DefType.Special;
                break;
            case "elastic":
                defType = DefType.Elastic;
                break;
        }
    }

    public void Step()
    {
        if(stateMachine == null)
        {
            stateMachine = new StateMachine(playerController);
        }
        stateMachine.updateState();

        if (playerController.isAttack && playerController.isGrounded)
        {
            charaData.activeMaxSpeed = charaData.maxSpeed * 0.4f;
        }
        else
        {
            charaData.activeMaxSpeed = charaData.maxSpeed;
        }

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

    public virtual void Attack()
    {
        Debug.Log("Attack in charactor");
        playerController.isAttack = true;
        playerController.lastAttackT = Time.time;

        var awaitGo = GameManager.InstantiateAsync(attackPref, playerController.transform.position);
        GameObject go = awaitGo;
        go.GetComponent<Attack>().CreateHandler(2, playerController.sawDir, atkType);

    }

    public virtual void Jump()
    {
        if (playerController.isGrounded)
        {
            playerController.body.AddForce(new Vector2(0, charaData.jumpForce), ForceMode2D.Impulse);
            playerController.isGrounded = false;
        }
    }

    public virtual void Up()
    {
        playerController.sawDir = new Vector2(playerController.presentSawDir.x, 1);
        
    }

    public virtual void Down()
    {
        if(playerController.isGrounded)
        {
            playerController.isSit = true;
        }
        else
        {
            playerController.sawDir = new Vector2(playerController.presentSawDir.x, -1);
        }
    }    

    public virtual void SpecialMove(int index)
    {

    }
}
