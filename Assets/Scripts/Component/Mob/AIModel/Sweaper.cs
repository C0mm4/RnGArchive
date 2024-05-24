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
            if(currentState == "MobIdle" || currentState == "MobMove")
            {
                target.SetTargetPosition(player.transform.position);
            }
            if (target.GetPlayerDistance() <= 0.8f)
            {
                // Attack Code Add
                if (!target.data.attackCooltime[0])
                {
                    target.ChangeState(new MobPrepareAttack(0));
                    Debug.Log("Mob Attack");
                }
            }
        }
    }

}
