using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40011000 : CutSceneTrigger
{
    public override async Task Action()
    {
        await base.Action();

        foreach(var chara in GameManager.Progress.currentParty)
        {
            Debug.Log(chara.charaData.id);
        }

        GameManager.Progress.DeleteCharaInParty(10001000);
        GameManager.Progress.InsertCharaInParty(10001001);

        Debug.Log("Charactor Set");

        foreach (var chara in GameManager.Progress.currentParty)
        {
            Debug.Log(chara.charaData.id);
        }

        await GameManager.Scene.StartGameAfterOpening();

    }
}
