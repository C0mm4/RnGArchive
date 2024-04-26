using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40012101 : CutSceneTrigger
{
    public Transform utahaSpawn;

    public Door Door;

    public override async Task Action()
    {
        PlayerController player = GameManager.Player.GetComponent<PlayerController>();
        FindNPC("20001001", utahaSpawn);
        await base.Action();
        GameManager.Progress.isActiveSupport = true;
    }

    public override bool AdditionalCondition()
    {
        foreach(var obj in conditionObjs)
        {
            if(obj != null)
            {
                return false;
            }
        }
        return true;
    }
}
