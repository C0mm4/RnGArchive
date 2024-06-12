using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Trigger : Obj
{
    public List<string> nodeIds;

    protected Collider2D triggerBox;

    public TriggerData data;

    public int SelectIndex;

    public TrigText trigText;

    public List<string> nextTriggerId;
    public List<Trigger> nextTrigger;

    public List<GameObject> conditionObjs;
    public List<GameObject> spawnObjs;

    public override void OnCreate()
    {
        base.OnCreate();
        triggerBox = GetComponent<Collider2D>();
        triggerBox.isTrigger = true;
        data.isActivate = false;
        conditionObjs = new();
        spawnObjs = new();
        if (GameManager.Progress.activeTrigs.ContainsKey(data.id))
        {
            data.isActivate = true;
        }
        else
        {
            data.isActivate = false;
        }
        string id = GetType().Name;
        id = id.Replace("Trig", "");
        data.id = id;
    }

    public async virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (data.startNPCId.Equals(""))
        {
            if (collision.gameObject == GameManager.player)
            {
                if (GameManager.GetUIState() == UIState.InPlay)
                {
                    if (!data.isActivate)
                    {
                        if (nodeIds.Count == 0)
                        {
                            if (AdditionalCondition())
                            {
                                await TriggerActive();
                            }
                        }
                        else
                        {
                            if (CheckNodesActive())
                            {
                                await TriggerActive();
                            }
                        }
                    }

                }
            }
        }
    }


    public virtual async Task TriggerActive()
    {
        StartCutScene(); 
        await Task.Run(() =>
        {
            GameManager.Trigger.ActiveTrigger(data);
        });
    }

    public bool CheckNodesActive()
    {
        foreach (string node in nodeIds)
        {
            if (!GameManager.Progress.activeTrigs.ContainsKey(node))
            {
                return false;
            }
        }
        return AdditionalCondition();

    }

    public abstract bool AdditionalCondition();


    public override void BeforeStep()
    {
        base.BeforeStep();
        if (GameManager.Progress.activeTrigs.ContainsKey(data.id))
        {
            data.isActivate = true;
        }
        else
        {
            data.isActivate = false;
        }
    }

    public void SetTriggerTextData(TrigText txts)
    {
        trigText = txts;
    }

    public async Task<int> GenSelectionUI(List<Script> scripts)
    {
        SelectUI ui = GameManager.InstantiateAsync("SelectUI").GetComponent<SelectUI>();
        List<string> selections = new List<string>();
        foreach(Script script in scripts)
        {
            selections.Add(script.script);
        }

        ui.CreateHandler(selections.Count, selections);
        Task<int> task = ui.Select();
        int ret;
        ret = await task;
        return ret;
    }

    public async Task NPCSay(Script script, NPC targetNPC)
    {
        string applyScript = Func.ChangeStringToValue(script.script);
        await targetNPC.Say(applyScript);
    }

    public async void StartCutScene()
    {
        PlayerController player = GameManager.player.GetComponent<PlayerController>();
        GameManager.ChangeUIState(UIState.CutScene);
        player.canMove = false;

        await Action();

        GameManager.ChangeUIState(UIState.InPlay);
        player.canMove = true;
    }

    public abstract Task Action();

    public NPC FindNPC(string id, Transform trans)
    {
        /*NPC ret = npcList.Find(item => item.npcId.Equals(id));*/
        List<NPC> npcs = GameManager.Instance.currentMapObj.GetComponentsInChildren<NPC>().ToList();
        NPC ret = npcs.Find(item => item.npcId.Equals(id));
        if(ret == null)
        {
            ret = NPCSpawn(id, trans);
        }
        return ret;

    }

    public NPC FindNPC(string id)
    {
        List<NPC> npcs = GameManager.Instance.currentMapObj.GetComponentsInChildren<NPC>().ToList();
        NPC ret = npcs.Find(item => item.npcId.Equals(id));

        return ret;
    }
    
    public NPC NPCSpawn(string id, Transform trans)
    {
        NPC ret = GameManager.MobSpawner.NPCSpawn(id, trans.position).GetComponent<NPC>();
        return ret;
    }

    public async Task ScriptPlay()
    {
        PlayerController currentChara = GameManager.player.GetComponent<PlayerController>();
        Debug.Log(currentChara.charactor.charaData.id);
        // if Current Player is Alice and Midori is Open, Spawn Midori NPC
        if (currentChara.charactor.charaData.id == 10001001)
        {
            if(GameManager.Progress.openCharactors.FindIndex(item => item == 10001002) != -1)
            {
                Debug.Log("Alice");
                PlayerController midori = GameManager.CutSceneCharactorSpawn(GameManager.player.transform, 10001002);
                await midori.ForceMove(GameManager.player.transform.position + new Vector3(-0.25f, 0) * GameManager.player.GetComponent<PlayerController>().sawDir.x);
                NPC midoriNPC = NPCSpawn("20001003", midori.transform);
                midoriNPC.transform.rotation = midori.transform.rotation;
                midori.Destroy();
            }

        }
        else
        {
            // if Alice is Open, Spawn Alice
            if(GameManager.Progress.openCharactors.FindIndex(item => item == 10001002) != -1)
            {
                PlayerController Alice = GameManager.CutSceneCharactorSpawn(GameManager.player.transform, 10001001);

            }

            // if current Charactor is Midori
            if(currentChara.charactor.charaData.id == 10001002)
            {
                await currentChara.ForceMove(GameManager.player.transform.position + new Vector3(-0.5f, 0) * GameManager.player.GetComponent<PlayerController>().sawDir.x);

                NPC midoriNPC = NPCSpawn("20001003", currentChara.transform);
                midoriNPC.transform.rotation = currentChara.transform.rotation;

            }

            
        }
        // if Current Player is Midori, Spawn Alice, and spawn NPC Midori

        /*else if (currentChara.charaData.id.Equals("10001002"))
        {

        }
*/
        // if Current Player is not Alice and Midori, Spawn Alice and Midori

        List<string> junctions = new List<string>();
        for (int i = 0; i < trigText.scripts.Count; i++)
        {
            if (!trigText.scripts[i].junctionID.Equals(""))
            {
                if(junctions.FindIndex(item => item.Equals(trigText.scripts[i].junctionID)) == -1)
                {
                    continue;
                }
            }
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
                int select = await GenSelectionUI(selections);

                // Select Split Point
                if (!selections[select].junction.Equals(""))
                {
                    junctions.Add(selections[select].junction);
                }
            }
            else if (npcId.Equals("99000000"))
            {
                await Func.Action(trigText.scripts[i].script);
            }
            else
            {
                NPC targetNPC = FindNPC(npcId);
                if (trigText.scripts[i].isAwait)
                {
                    #pragma warning disable CS4014 
                    NPCSay(trigText.scripts[i], targetNPC);
                    #pragma warning restore CS4014 
                    await Task.Delay(TimeSpan.FromSeconds(trigText.scripts[i].delayT));
                }
                await NPCSay(trigText.scripts[i], targetNPC);
            }
            if (!trigText.scripts[i].subTriggerId.Equals(""))
            {
                Type t = Type.GetType("SubTrig" + trigText.scripts[i].subTriggerId);
                SubTrigger subTrigger;
                subTrigger = Activator.CreateInstance(t) as SubTrigger;
                subTrigger.originTrig = this;
                await subTrigger.Action();
            }
        }
    }

    public void ActiveSpawnObjs()
    {
        foreach(GameObject go in spawnObjs)
        {
            go.GetComponent<Mob>().isForceMoving = false;
            go.GetComponent<Mob>().canMove = true;
        }
    }

}
