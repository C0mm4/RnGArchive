using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trigger : Obj
{
    public List<string> nodeIds;

    protected Collider2D triggerBox;

    public TriggerData data;

    public override void OnCreate()
    {
        base.OnCreate();
        triggerBox = GetComponent<Collider2D>();
        triggerBox.isTrigger = true;
        data.isActivate = false;
    }

    public async void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.player)
        {
            if (!data.isActivate)
            {
                if(nodeIds.Count == 0)
                {
                    await TriggerActive();
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

    public virtual async Task TriggerActive()
    {
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
            if (!GameManager.Trigger.activeTriggerLists.ContainsKey(node))
            {
                return false;
            }
        }
        return true;
    }

    public override void Step()
    {
        base.Step();
        if (GameManager.Trigger.activeTriggerLists.ContainsKey(data.id))
        {
            data.isActivate = true;
        }
        else
        {
            data.isActivate = false;
        }
    }
}
