using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightLight : Skill
{
    private float time;

    public override void Execute(Vector2 dir)
    {
        base.Execute(dir);
        time = Time.time;

    }

    public override void PassiveEffect()
    {

    }

    public override void Step()
    {
        if (Input.GetKeyUp(GameManager.Input._keySettings.Shot))
        {
            float charT = Time.time - time;
            if(charT >= 5f)
            {
                Debug.Log("FullCharge");
            }
            else if(Time.time - time >= 3f)
            {
                Debug.Log("Charge");
            }
            End();
        }
    }
}
