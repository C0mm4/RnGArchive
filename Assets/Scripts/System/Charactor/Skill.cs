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
            if (currentAmmo > 0)
            {
                player = GameManager.player.GetComponent<PlayerController>();
                player.workingSkill = this;
                currentAmmo--;
                isCool = true;
                leftCoolTime = coolTime;
                Debug.Log(GetType().Name);
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
}
