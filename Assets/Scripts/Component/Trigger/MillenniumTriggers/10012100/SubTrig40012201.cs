using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig40012201 : SubTrigger
{
    public override async Task Action()
    {
        FindOriginTrig();
        Door door = originTrig.GetComponent<Trig40012101>().Door;
        GameManager.CameraManager.CameraMove(door.transform);
        var pos = door.transform.position;
        pos.x += door.InDir.x * 1.5f * door.transform.localScale.x;

#pragma warning disable CS4014
        GameObject[] gos = new GameObject[4];
        GameObject go;
        for (int k = 0; k < 4; k++)
        {
            await Task.Delay(TimeSpan.FromSeconds(1f));
            go = GameManager.MobSpawner.MobSpawn("FlyingDrone", pos);
            Vector3 movePos = door.transform.position;
            movePos.x += -door.InDir.x * ((door.InDir.x * 1.25f) + (k * 0.9f));
            go.GetComponent<Mob>().isForceMoving = true;
            go.GetComponent<Mob>().SetTargetPosition(movePos);
            gos[k] = go;
            originTrig.nextTrigger[0].conditionObjs.Add(go);
        }
#pragma warning restore CS4014
        await Task.Delay(TimeSpan.FromSeconds(1f));

        SetOriginSpawnObjs(gos);
        GameManager.CameraManager.CameraMove(GameManager.player.transform);
    }

}
