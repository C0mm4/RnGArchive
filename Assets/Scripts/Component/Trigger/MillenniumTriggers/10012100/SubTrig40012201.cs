using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig40012201 : SubTrigger
{
    public override async Task Action()
    {
        SpawnP door = GameManager.Stage.currentMap.FindSpawnP("Door");
        GameManager.CameraManager.CameraMove(door.transform);

        var pos = door.transform.position;

#pragma warning disable CS4014
        GameObject[] gos = new GameObject[4];
        GameObject go;
        for (int k = 0; k < 4; k++)
        {
            await Task.Delay(TimeSpan.FromSeconds(1f));
            go = GameManager.MobSpawner.MobSpawn("FlyingDrone", pos);
            go.GetComponent<Mob>().SetTargetPosition(door.transform.position + new Vector3(-0.7f, 0) * k);
            go.GetComponent<Mob>().isForceMoving = true;
            gos[k] = go;
            originTrig.nextTrigger[0].conditionObjs.Add(go);
        }
#pragma warning restore CS4014
        await Task.Delay(TimeSpan.FromSeconds(1f));

        SetOriginSpawnObjs(gos);
        GameManager.CameraManager.CameraMove(GameManager.player.transform);
    }

}
