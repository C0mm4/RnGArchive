using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ArtPolicing : Skill
{
    public override async void Action()
    {
        GameObject HealEffect = GameManager.InstantiateAsync("MidoriHeal", player.transform.position, player.transform.rotation);
        HealEffect.transform.SetParent(player.transform, true);
        player.HPIncrease(1);
        await Task.Delay(TimeSpan.FromSeconds(0.3f));

        GameManager.Destroy(HealEffect);
    }

    public override bool ExecuteCondition()
    {
        CharactorData data = player.charactor.charaData;
        if(data.currentHP < data.maxHP && player.isGrounded)
        {
            return true;

        }
        else
        {
            return false;
        }
        
    }

    public override void PassiveEffect()
    {
    }

    public override void Step()
    {
    }

}
