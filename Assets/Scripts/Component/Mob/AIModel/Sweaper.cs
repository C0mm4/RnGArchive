using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweaper : AIModel
{
    public override void Step()
    {
        if(!target.isForceMoving)
        {
            target.moveTargetPos = player.transform.position;
        }
    }

}
