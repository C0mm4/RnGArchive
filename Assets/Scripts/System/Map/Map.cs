using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : Obj
{
    public SpriteRenderer bound;

    public override void OnCreate()
    {
        base.OnCreate();
        SetTriggerDatas();
    }

    public void SetTriggerDatas()
    {
        Trigger[] triggers = GetComponentsInChildren<Trigger>();

        foreach (var trigger in triggers)
        {
            foreach(string nextId in trigger.nextTriggerId)
            {
                foreach(var trig2 in triggers)
                {
                    if (trig2.data.id.Equals(nextId))
                    {
                        trigger.nextTrigger.Add(trig2);
                    }
                }
            }
        }
    }

    public SpawnP FindSpawnP(string id)
    {
        SpawnP ret;
        List<SpawnP> list = GetComponentsInChildren<SpawnP>().ToList();
        ret = list.Find(item => item.id == id);
        
        return ret;

    }
}
