using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig40012200 : SubTrigger
{
    public async override Task Action()
    {
        Door door = originTrig.GetComponent<Trig40012100>().Door;
        GameManager.CameraManager.CameraMove(door.transform);
        var pos = door.transform.position;

#pragma warning disable CS4014
        GameObject[] gos = new GameObject[4];
        GameObject go;
        for (int i = 0; i < 4; i++)
        {
            await Task.Delay(TimeSpan.FromSeconds(1f));
            go = GameManager.MobSpawner.MobSpawn("Sweaper", pos, true);
            Vector3 movePos = door.transform.position;
            movePos.x += -door.InDir.x * ((door.InDir.x * 1.25f) + (i * 0.9f));
            go.GetComponent<Mob>().SetTargetPosition(movePos);
            go.GetComponent<Mob>().isForceMoving = true;
            gos[i] = go;
            originTrig.nextTrigger[0].conditionObjs.Add(go);
        }
#pragma warning restore CS4014
        await Task.Delay(TimeSpan.FromSeconds(1f));

        SetOriginSpawnObjs(gos);
        GameManager.CameraManager.CameraMove(GameManager.player.transform);
    }
}
