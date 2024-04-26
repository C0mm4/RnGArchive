using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CutSceneTrigger : Trigger
{
    public override async Task Action()
    {
        await ScriptPlay();
        ActiveSpawnObjs();
    }

    public override bool AdditionalCondition()
    {
        return true;
    }

}
