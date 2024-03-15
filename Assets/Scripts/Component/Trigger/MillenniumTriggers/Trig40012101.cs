using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40012101 : Trigger
{
    public List<GameObject> gameObjects = new();
    public Transform utahaSpawn;

    public Door Door;

    public Trigger nextTrigger;

    public override async Task Action()
    {
        PlayerController player = GameManager.Player.GetComponent<PlayerController>();
        NPCSpawn("20001001", utahaSpawn);
        for (int i = 0; i < trigText.scripts.Count; i++)
        {
            string npcId = trigText.scripts[i].npcId;
            if (npcId.Equals("90000000"))
            {
                List<Script> selections = new List<Script>();
                selections.Add(trigText.scripts[i]);
                if (i != trigText.scripts.Count - 1)
                {
                    for (int j = i + 1; j < trigText.scripts.Count && trigText.scripts[j].npcId.Equals("90000001"); j++)
                    {
                        selections.Add(trigText.scripts[j]);
                        i++;
                    }
                }
                await GenSelectionUI(selections);
            }
            else
            {
                NPC targetNPC = FindNPC(npcId);
                await NPCSay(trigText.scripts[i], targetNPC);
            }
            if (trigText.scripts[i].script.Equals("계속해서 드론을 보낼게"))
            {
                GameManager.CameraManager.CameraMove(Door.transform);
                var pos = Door.transform.position;
                pos.x += Door.InDir.x * 1.5f * Door.transform.localScale.x;

#pragma warning disable CS4014
                GameObject[] gos = new GameObject[4];
                GameObject go;
                for (int k = 0; k < 4; k++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                    go = GameManager.MobSpawner.MobSpawn("Sweaper", pos);
                    Vector3 movePos = Door.transform.position;
                    movePos.x += -Door.InDir.x * ((Door.InDir.x * 1.25f) + (i * 0.9f));
                    gos[k] = go;
                    nextTrigger.GetComponent<Trig40012102>().gameObjects.Add(go);
                }
#pragma warning restore CS4014
                await Task.Delay(TimeSpan.FromSeconds(1f));

                GameManager.CameraManager.CameraMove(player.transform);
            }
        }
    }

    public override bool AdditionalCondition()
    {
        foreach(var obj in gameObjects)
        {
            if(obj != null)
            {
                return false;
            }
        }
        return true;
    }
}
