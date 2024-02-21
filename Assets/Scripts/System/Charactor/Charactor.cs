using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Charactor
{
    public Dictionary<List<KeyValues>, Skill> commands;
    public List<Skill> passiveSkill;

    public Animator bodyAnimator;
    public Animator legAnimator;

    public StateMachine stateMachine;

    public SerializeStatus status;

    public PlayerController playerController;

    public string attackPref;

    public void Step()
    {
        if(stateMachine == null)
        {
            stateMachine = new StateMachine(playerController);
        }
        stateMachine.updateState();

        if (playerController.isAttack && playerController.IsGrounded)
        {
            status.activeMaxSpeed = status.maxSpeed * 0.4f;
        }
        else
        {
            status.activeMaxSpeed = status.maxSpeed;
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

        Addressables.InstantiateAsync(attackPref, position: playerController.transform.position, rotation: playerController.transform.rotation).Completed += handle =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                GameObject go = handle.Result;
                go.GetComponent<Obj>().handle = handle;
                go.GetComponent<Attack>().CreateHandler(2, playerController.sawDir);
            });
        };
    }

    public virtual void Jump()
    {
        if (playerController.IsGrounded)
        {
            playerController.velocity.y += playerController.jumpTakeOffSpeed;
            playerController.IsGrounded = false;
            playerController.jumpTime = 0;
        }
    }

    public virtual void Up()
    {
        playerController.sawDir = new Vector2(playerController.presentSawDir.x, 1);
        
    }

    public virtual void Down()
    {
        playerController.sawDir = new Vector2(playerController.presentSawDir.x, -1);
    }    

    public virtual void SpecialMove(int index)
    {

    }
}
