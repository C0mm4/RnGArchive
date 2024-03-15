using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Trigger : Obj
{
    public List<string> nodeIds;

    protected Collider2D triggerBox;

    public TriggerData data;

    public int SelectIndex;

    public TrigText trigText;
    public List<NPC> npcList;

    public override void OnCreate()
    {
        base.OnCreate();
        triggerBox = GetComponent<Collider2D>();
        triggerBox.isTrigger = true;
        data.isActivate = false;
        if (GameManager.Progress.activeTrigs.ContainsKey(data.id))
        {
            data.isActivate = true;
        }
        else
        {
            data.isActivate = false;
        }
    }

    public async void OnTriggerStay2D(Collider2D collision)
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


    public virtual async Task TriggerActive()
    {
        StartCutScene(); 
        await Task.Run(() =>
        {
            Debug.Log(data.id);
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
        targetNPC.SetScript(script.script);
        await targetNPC.Say();
        targetNPC.SetScript("");
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

    public NPC FindNPC(string id)
    {
        return npcList.Find(item => item.npcId.Equals(id));

    }
    
    public void NPCSpawn(string id, Transform trans)
    {
        NPC ret = GameManager.MobSpawner.NPCSpawn(id, trans.position).GetComponent<NPC>();
        npcList.Add(ret);
    }
}
