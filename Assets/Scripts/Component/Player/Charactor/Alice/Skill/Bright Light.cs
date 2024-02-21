using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightLight : Skill
{
    public override void Execute(Vector2 dir)
    {
        base.Execute(dir);
        Debug.Log("AA");
    }

    public override void PassiveEffect()
    {

    }

    public override void Step()
    {

    }

    public override void PassiveStep()
    {
        base.PassiveStep();
        if (player.isSit)
        {
            if (Input.GetKeyDown(GameManager.Input._keySettings.Shot))
            {
                Execute(player.sawDir);
            }
        }

    }
}
