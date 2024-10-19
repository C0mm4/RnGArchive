using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Trigger : Obj
{
    public List<string> nodeIds = new();

    protected Collider2D triggerBox;

    public TriggerData data = new("00000000", false);

    public TrigText trigText;

    public List<string> nextTriggerId = new();
    public List<Trigger> nextTrigger;

    public List<GameObject> conditionObjs;
    public List<GameObject> spawnObjs;

    public bool isInitialize = false;

    [Serializable]

    public class AdditionalCondi
    {
        [SerializeField]
        public enum condiType 
        {
            None, CheckMobAlive, CheckMobHP, 
        };

        [SerializeField]
        public condiType condition;

        [SerializeField]
        public float HPRatio;

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
    public AdditionalCondi condi = new();

    public enum TriggerType
    {
        CutScene, Spawn,
    }

    public TriggerType type = TriggerType.CutScene;

    public override void OnCreate()
    {
        if (!GameManager.isPaused)
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        base.OnCreate();
        triggerBox = GetComponent<Collider2D>();
        if (triggerBox != null)
        {
            triggerBox.isTrigger = true;
        }
        data.isActivate = false;
        conditionObjs = new();
        spawnObjs = new();
        if (GameManager.Progress != null)
        {
            if (GameManager.Progress.activeTrigs.ContainsKey(data.id))
            {
                data.isActivate = true;
            }
            else
            {
                data.isActivate = false;
            }
        }
        condi.originTrigger = this;
        isInitialize = true;
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

        GameManager.Stage.RefreshNPCScript();
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
        if (isInitialize)
        {
            if (GameManager.Progress.activeTrigs.ContainsKey(data.id))
            {
                data.isActivate = true;
            }
            else
            {
                data.isActivate = false;
            }
        }
        else
        {
            if (!GameManager.isPaused)
            {
                Initialize();
            }
        }
    }

    public void SetTriggerTextData(TrigText txts)
    {
        trigText = txts;

        if (GameManager.Progress.activeTrigs.ContainsKey(data.id))
        {
            data.isActivate = true;
        }
        else
        {
            data.isActivate = false;
        }

        // is this trigger is activate already
        if (data.isActivate)
        {
            // Add NPC Spawn Despawn Data for Map Load
            foreach (Script script in trigText.scripts)
            {
                if (script.npcId.Equals("99000000"))
                {
#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
                    Func.Action(script.script);
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
                }
            }

        }
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
        GameManager.ChangeUIState(UIState.CutScene);
        PlayerController currentChara = GameManager.player.GetComponent<PlayerController>();

        List<PlayerController> playerDummys = new();
        List<NPC> spawnNPCs = new();

        int state = 0;

        currentChara.velocity = new Vector2(0, currentChara.velocity.y);

        // Add NPC Spawn in Script Play

/*        if(!currentChara.charactor.charaData.id.Equals("10001001"))
        {
            if(GameManager.Progress.openCharactors.FindIndex(item => item.Equals("10001001")) != -1)
            {
                PlayerController dummy = GameManager.CutSceneCharactorSpawn(currentChara.transform, "10001001");
                playerDummys.Add(dummy);

                state = 1;
            }
        }

        foreach(Charactor chara in GameManager.Progress.currentParty)
        {
            // is Not Alice
            if(chara.charaData.id != currentChara.charactor.charaData.id && !chara.charaData.id.Equals("10001001"))
            {
                if (GameManager.Progress.charaDatas["10001001"].isOpen) 
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
        }*/


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
                GameManager.CameraManager.player = GameManager.player.transform;
                await Task.Delay(TimeSpan.FromSeconds(1));
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
                GameManager.CameraManager.player = targetNPC.transform;
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
                Type t = Type.GetType("SubTrig" + trigText.scripts[i].subTriggerId);
                SubTrigger subTrigger;
                subTrigger = Activator.CreateInstance(t) as SubTrigger;
                subTrigger.originTrig = this;
                await subTrigger.Action();
            }
        }
/*
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
        }*/
        //        playerDummys.Clear();

        GameManager.CameraManager.player = GameManager.player.transform;
    }

    public void ActiveSpawnObjs()
    {
        foreach(GameObject go in spawnObjs)
        {
            go.GetComponent<Mob>().isForceMoving = false;
            go.GetComponent<Mob>().canMove = true;
        }
    }

    public void drawLine(Color color)
    {
        LineRenderer line = GetComponent<LineRenderer>();
        if(line == null)
            line = gameObject.AddComponent<LineRenderer>();

        line.loop = true;
        Vector3[] positions = new Vector3[5];

        positions[0] = transform.position + new Vector3(-GetComponent<BoxCollider2D>().size.x / 2, GetComponent<BoxCollider2D>().size.y / 2);
        positions[1] = transform.position + new Vector3(GetComponent<BoxCollider2D>().size.x / 2, GetComponent<BoxCollider2D>().size.y / 2);
        positions[2] = transform.position + new Vector3(GetComponent<BoxCollider2D>().size.x / 2, -GetComponent<BoxCollider2D>().size.y / 2);
        positions[3] = transform.position + new Vector3(-GetComponent<BoxCollider2D>().size.x / 2, -GetComponent<BoxCollider2D>().size.y / 2);
        positions[4] = positions[0];

        line.startColor = color;
        line.endColor = color;

        line.startWidth = 0.01f;
        line.endWidth = 0.01f;

        line.positionCount = 5; 

        line.useWorldSpace = true;

        line.SetPositions(positions);
        
    }
}
