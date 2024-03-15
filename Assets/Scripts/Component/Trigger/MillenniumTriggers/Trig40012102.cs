using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40012102 : Trigger
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

            if (trigText.scripts[i].script.Equals("해당 드론의 내구성 테스트도 겸할 겸 그 아이와도 전투를 치뤄봐"))
            {
                var pos = Door.transform.position;
                pos.x += Door.InDir.x * 1.5f * Door.transform.localScale.x;

                GameObject go = await GameManager.MobSpawner.BossSpawn("SweaperBoss", pos);
                nextTrigger.GetComponent<Trig40012103>().gameObjects.Add(go);
            }
            await Task.Yield();
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
