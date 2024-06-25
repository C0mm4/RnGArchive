using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig4001220 : SubTrigger
{
    public override async Task Action()
    {
        GameManager.Save.SaveGameprogress(GameManager.Player.transform);
        GameManager.ChangeUIState(UIState.CutScene);
        await GameManager.Scene.FadeIn();
        await Task.Yield();
    }
}
