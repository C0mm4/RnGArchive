using JetBrains.Annotations;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    [Serializable]

    class AdditionalCondi
    {
        [SerializeField]
        enum condiType 
        {
            None, CheckMobAlive, CheckMobHP, 
        };

        [SerializeField]
        condiType condition;

        [SerializeField]
        float HPRatio;

        public Trigger originTrigger;

        public bool CheckCondi()
        {
            switch (condition)
            {
                case condiType.None: return true;
                case condiType.CheckMobAlive:
                    foreach (var item in originTrigger.conditionObjs)
                    {
                        if (item != null)
                            return false;
                    }
                    return true;
                case condiType.CheckMobHP:
                    var mob = originTrigger.conditionObjs[0].GetComponent<Mob>();
                    if(mob.status.currentHP < mob.status.maxHP * HPRatio)
                    {
                        return true;
                    }
                    return false;
                default: return true;
            }
        }

    }
    [SerializeField]
    AdditionalCondi condi;

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
        }/*
        string id = GetType().Name;
        id = id.Replace("Trig", "");
        data.id = id;*/
        condi.originTrigger = this;
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
                            if (condi.CheckCondi())
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
        await StartCutScene();
        
        await Task.Run(() =>
        {
            GameManager.Trigger.ActiveTrigger(data);
        });
//        GameManager.Trigger.ActiveTrigger(data);
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
        return condi.CheckCondi();

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

    public async Task StartCutScene()
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

        List<PlayerController> playerDummys = new();
        List<NPC> spawnNPCs = new();

        int state = 0;


        if(currentChara.charactor.charaData.id != 10001001)
        {
            if(GameManager.Progress.openCharactors.FindIndex(item => item == 10001001) != -1)
            {
                PlayerController dummy = GameManager.CutSceneCharactorSpawn(currentChara.transform, 10001001);
                playerDummys.Add(dummy);

                state = 1;
            }
        }

        foreach(Charactor chara in GameManager.Progress.currentParty)
        {
            // is Not Alice
            if(chara.charaData.id != currentChara.charactor.charaData.id && chara.charaData.id != 10001001)
            {
                if (GameManager.Progress.charaDatas[10001001].isOpen) 
                {
                    PlayerController dummy = GameManager.CutSceneCharactorSpawn(currentChara.transform, chara.charaData.id);
                    playerDummys.Add(dummy);
                }
            }
        }

        if(playerDummys.FindIndex(item => item.charactor.charaData.id == 10001002) == 1)
        {
            if(GameManager.Progress.openCharactors.FindIndex(item => item == 10001002) != -1)
            {
                PlayerController dummy = GameManager.CutSceneCharactorSpawn(currentChara.transform, 10001002);
                if(state == 0)
                {
                    playerDummys.Insert(0, dummy);
                }
                else
                {
                    playerDummys.Insert(1, dummy);
                }
            }
        }

        for(int i = 0; i < playerDummys.Count; i++)
        {
            Transform dummyTrans = playerDummys[i].transform;
            int index = i;
            UnityMainThreadDispatcher.Instance().Enqueue(async () =>
            {
                await playerDummys[index].ForceMove(dummyTrans.position + new Vector3(-0.3f * (index + 1), 0) * currentChara.sawDir.x);

                await Task.Delay(TimeSpan.FromMilliseconds(10));

                string NPCId = Func.PlayerIDToNPCID(playerDummys[index].charactor.charaData.id.ToString());
                if(NPCId != null)
                {
                    NPC npc = NPCSpawn(NPCId, playerDummys[index].transform);
                    npc.transform.rotation = playerDummys[index].transform.rotation;
                    spawnNPCs.Add(npc);

                    playerDummys[index].gameObject.SetActive(false);
                }
            });
        }


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
                    await Task.Delay(TimeSpan.FromMilliseconds(trigText.scripts[i].delayT));
                }
                else
                {
                    await NPCSay(trigText.scripts[i], targetNPC);
                }
            }
            if (!trigText.scripts[i].subTriggerId.Equals(""))
            {
                Debug.Log(trigText.scripts[i].subTriggerId);
                Type t = Type.GetType("SubTrig" + trigText.scripts[i].subTriggerId);
                SubTrigger subTrigger;
                subTrigger = Activator.CreateInstance(t) as SubTrigger;
                subTrigger.originTrig = this;
                await subTrigger.Action();
            }
        }

        for(int i = 0; i < spawnNPCs.Count; i++)
        {
            spawnNPCs[i].Destroy();
        }
        spawnNPCs.Clear();

        for(int i = 0; i < playerDummys.Count;  i++)
        {
            playerDummys[i].gameObject.SetActive(true);
            int index = i;
            UnityMainThreadDispatcher.Instance().Enqueue(async () =>
            {
                await playerDummys[index].ForceMove(currentChara.transform.position, false);

                playerDummys[index].Destroy();
            });
        }
//        playerDummys.Clear();
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
