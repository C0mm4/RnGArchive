using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SubTrigger
{
    public Trigger originTrig;


    public abstract Task Action();
    
    public void SetOriginSpawnObjs(GameObject[] objs)
    {
        foreach(GameObject obj in objs)
        {
            originTrig.spawnObjs.Add(obj);

        }
    }

    public void SetOriginSpawnObjs(GameObject go)
    {
        originTrig.spawnObjs.Add(go);
    }
}
