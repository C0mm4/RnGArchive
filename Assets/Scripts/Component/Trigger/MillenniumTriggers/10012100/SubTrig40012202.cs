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
        SpawnP door = GameManager.Stage.currentMap.FindSpawnP("Door");
        var pos = door.transform.position;

        GameObject go = await GameManager.MobSpawner.BossSpawn("SweaperBoss", pos);

        foreach(var item in originTrig.nextTrigger)
        {
            item.conditionObjs.Add(go);

        }

        SetOriginSpawnObjs(go);

        await Task.Delay(TimeSpan.FromSeconds(1f));
    }

}
