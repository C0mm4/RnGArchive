using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Skill
{
    public PlayerController player;
    public string name;

    public int cost;

    public float coolTime = 5;
    public bool isCool = false;
    public float leftCoolTime;

    public Skill()
    {
    }

    public abstract void PassiveEffect();
    public virtual void Execute(Vector2 dir)
    {
        if (!isCool)
        {
            player = GameManager.player.GetComponent<PlayerController>();
            if (player.charactor.charaData.currentCost >= cost && ExecuteCondition())
            {
                player.workingSkill = this;
                isCool = true;
                leftCoolTime = coolTime;
                Debug.Log(GetType().Name);
                player.charactor.charaData.currentCost -= cost;

                Action();
            }
        }
    }

    public abstract void Step();

    public virtual void End()
    {

    }

    public virtual void PassiveStep()
    {
        if(player == null)
        {
            player = GameManager.player.GetComponent<PlayerController>();
        }
    }

    public abstract bool ExecuteCondition();

    public abstract void Action();
}
