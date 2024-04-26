using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40012103 : CutSceneTrigger
{
    public Transform utahaSpawn;

    public override async Task Action()
    {
        PlayerController player = GameManager.Player.GetComponent<PlayerController>();
        FindNPC("20001001", utahaSpawn);
        conditionObjs[0].GetComponent<Mob>().canMove = false;
        await base.Action();

        conditionObjs[0].GetComponent<Mob>().canMove = true;
    }

    public override bool AdditionalCondition()
    {
        if(conditionObjs.Count == 0)
        {
            return false;
        }
        if (conditionObjs[0].GetComponent<Mob>().isSet)
        {
            if (conditionObjs[0].GetComponent<Mob>().status.currentHP <= 35 && conditionObjs[0].GetComponent<Mob>().status.maxHP == 40)
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }

    }
}
