using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40012103 : Trigger
{
    public List<GameObject> gameObjects = new();
    public Transform utahaSpawn;

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
            
        }
        GameManager.Progress.isActiveSkill = true;
        nextTrigger.GetComponent<Trig40012104>().gameObjects.AddRange(gameObjects);
    }

    public override bool AdditionalCondition()
    {
        if(gameObjects.Count == 0)
        {
            return false;
        }
        if (gameObjects[0].GetComponent<Mob>().isSet)
        {
            if (gameObjects[0].GetComponent<Mob>().status.currentHP <= 35 && gameObjects[0].GetComponent<Mob>().status.maxHP == 40)
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }

    }
}
