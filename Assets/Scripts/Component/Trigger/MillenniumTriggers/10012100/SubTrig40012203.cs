using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig40012203 : SubTrigger
{
    public override async Task Action()
    {
        GameManager.Progress.isActiveSkill = true;
        await Task.Yield();
    }

}
