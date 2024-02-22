using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public PlayerController player;
    public bool isPassive;
    public string name;

    public Skill()
    {
    }

    public abstract void PassiveEffect();
    public virtual void Execute(Vector2 dir)
    {
        player = GameManager.player.GetComponent<PlayerController>();
        player.workingSkill = this;
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
}
