using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBreakdown : Skill
{

    public override void Execute(Vector2 dir)
    {
        base.Execute(dir);
        player.charactor.ChangeState(new SpecialMove(2f, 1));
        player.isAction = true;
    }

    public override void PassiveEffect()
    {

    }

    public override void Step()
    {

    }
}
