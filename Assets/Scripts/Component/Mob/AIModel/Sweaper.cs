using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweaper : AIModel
{
    public override void Step()
    {
        if(!target.isForceMoving)
        {
            target.SetTargetPosition(player.transform.position);
            if (target.GetPlayerDistance() <= 0.2f)
            {
                Debug.Log("A");
                // Attack Code Add
                target.moveAccel *= 0;
            }
        }
    }

}
