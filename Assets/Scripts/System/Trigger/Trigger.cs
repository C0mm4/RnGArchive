using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : Obj
{
    public List<string> nodeIds;

    protected Collider2D triggerBox;

    public string id;
    public bool isActivate;

    public override void OnCreate()
    {
        base.OnCreate();
        triggerBox = GetComponent<Collider2D>();
        triggerBox.isTrigger = true;
        isActivate = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.player)
        {
            if (!isActivate)
            {
                if(nodeIds.Count == 0)
                {
                    TriggerActive();
                }
                else
                {
                    if (CheckNodesActive())
                    {
                        TriggerActive();
                    }
                }
            }
        }
    }

    public virtual void TriggerActive()
    {
        Debug.Log(id);
        GameManager.Trigger.ActiveTrigger(this);
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
}
