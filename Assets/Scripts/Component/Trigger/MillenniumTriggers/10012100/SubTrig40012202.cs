using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig40012202 : SubTrigger
{
    public override async Task Action()
    {
        Debug.Log("40012202 is Activate");
        Door door = originTrig.GetComponent<Trig40012102>().Door;
        var pos = door.transform.position;
        pos.x += door.InDir.x * 1.5f * door.transform.localScale.x;

        GameObject go = await GameManager.MobSpawner.BossSpawn("SweaperBoss", pos);

        foreach(var item in originTrig.nextTrigger)
        {
            item.conditionObjs.Add(go);

        }

        SetOriginSpawnObjs(go);

        await Task.Delay(TimeSpan.FromSeconds(1f));
    }

}
