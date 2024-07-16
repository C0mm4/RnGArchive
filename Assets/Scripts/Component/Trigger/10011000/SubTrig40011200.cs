using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig40011200 : SubTrigger
{
    public override async Task Action()
    {
        GameManager.Progress.DeleteCharaInParty(10001000);
        GameManager.Progress.InsertCharaInParty(10001001);
        GameManager.Progress.InsertCharaInParty(10001002);

        await GameManager.Scene.StartGameAfterOpening();
    }
}
