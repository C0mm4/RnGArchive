using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Skill
{
    public PlayerController player;
    public bool isPassive;
    public string name;

    public int maxAmmo;
    public int currentAmmo;

    public Skill()
    {
    }

    public abstract void PassiveEffect();
    public virtual void Execute(Vector2 dir)
    {
        if(currentAmmo > 0)
        {
            player = GameManager.player.GetComponent<PlayerController>();
            player.workingSkill = this;
            currentAmmo--;
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
}
