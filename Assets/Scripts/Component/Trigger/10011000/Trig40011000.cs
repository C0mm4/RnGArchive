using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trig40011000 : CutSceneTrigger
{
    public override async Task Action()
    {
        await base.Action();


        GameManager.Progress.DeleteCharaInParty(10001000);
        GameManager.Progress.InsertCharaInParty(10001001);


        await GameManager.Scene.StartGameAfterOpening();

    }
}
