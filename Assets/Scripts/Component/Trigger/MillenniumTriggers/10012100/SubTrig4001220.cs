using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubTrig4001220 : SubTrigger
{
    public override async Task Action()
    {
        await GameManager.Scene.FadeIn();
        await Task.Yield();
    }
}