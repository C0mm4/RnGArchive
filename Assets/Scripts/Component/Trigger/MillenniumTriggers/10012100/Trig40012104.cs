using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40012104 : CutSceneTrigger
{
    public Transform utahaSpawn;
    public Door door;

    public override async Task Action()
    {
        FindNPC("20001001", utahaSpawn);
        await base.Action();
        door.isActivate = true;
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
