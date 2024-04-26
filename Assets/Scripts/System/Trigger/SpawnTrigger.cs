using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnTrigger : Trigger
{
    public List<string> mobs = new();
    public List<Transform> Trans = new();

    public override async Task Action()
    {
        await Task.Yield();
        for(int i = 0; i < mobs.Count; i++)
        {
            GameManager.MobSpawner.MobSpawn(mobs[i], Trans[i].position);
            
        }
    }

    public override void BeforeStep()
    {
    }

    public override bool AdditionalCondition()
    {
        return true;
    }
    public override async Task TriggerActive()
    {
        data.isActivate = true;
        await Action();
        
    }


    public async override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.player)
        {
            if (GameManager.GetUIState() == UIState.InPlay)
            {
                if (!data.isActivate)
                {
                    data.isActivate = true;
                    await TriggerActive();
                }

            }
        }
    }
}
