using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweaper : AIModel
{
    public override void Step()
    {
        string currentState = target.currentState;

        if(!target.isForceMoving)
        {
            if(currentState == "Idle" || currentState == "MobMove")
            {
                target.SetTargetPosition(player.transform.position);
            }
            if (target.GetPlayerDistance() <= 0.8f)
            {
                // Attack Code Add

                target.ChangeState(new MobAttack(0));
            }
        }
    }

}
