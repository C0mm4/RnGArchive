using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40012200 : Trigger
{
    public Door outDoor;
    public async override Task Action()
    {
        await ScriptPlay();
        ActiveSpawnObjs();
        outDoor.isActivate = true;
    }

    public override bool AdditionalCondition()
    {
        return true;
    }

}
