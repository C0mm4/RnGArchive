using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Skill
{
    public PlayerController player;

    public string imgPath;
    public string name;
    public string type;
    public string info;


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
            if (ExecuteCondition())
            {
                isCool = true;
                leftCoolTime = coolTime;
                Debug.Log(GetType().Name);

                Action();
            }
        }
    }

    public virtual void Step()
    {
    }

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
