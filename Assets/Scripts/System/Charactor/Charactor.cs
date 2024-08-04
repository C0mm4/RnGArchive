using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class Charactor : Contents
{
    public Dictionary<List<KeyValues>, Skill> commands;
    [SerializeField]
    public List<Skill> passiveSkill;

    public List<Skill> skills;


    public StateMachine stateMachine;

    public CharactorData charaData;

    public PlayerController playerController;

    public string weaponPref;

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
        charaData.activeMaxSpeed = charaData.maxSpeed;

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
        playerController.isAttack = true;

        playerController.weapon.Fire();
/*        var awaitGo = GameManager.InstantiateAsync(attackPref, playerController.transform.position);
        GameObject go = awaitGo;
        go.GetComponent<Attack>().CreateHandler(2, playerController.sawDir, atkType);*/

    }

    public virtual void Jump()
    {
        if (playerController.isGrounded)
        {
            playerController.AddForce(new Vector2(0, charaData.jumpForce));
            playerController.isGrounded = false;
        }
    }


}
