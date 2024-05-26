using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Sweaper : AIModel
{
    public override void Step()
    {
        string currentState = target.currentState;

        switch (currentState)
        {
            case "MobIdle":
                if (target != null)
                {
                    if (!target.isGrounded)
                    {
                        if (target.velocity.y > 0)
                        {
                            target.ChangeState(new MobPrepareJump());
                        }
                        else
                        {
                            target.ChangeState(new MobFalling());
                        }
                    }
                    else if (Mathf.Abs(target.velocity.x) > 0.01f)
                    {
                        target.ChangeState(new MobMove());
                    }
                }
                break;
            case "MobMove":
                break;

        }

        if (!target.isForceMoving)
        {
            if(currentState == "MobIdle" || currentState == "MobMove")
            {
                target.SetTargetPosition(player.transform.position);
            }
            if (target.GetPlayerDistance() <= 0.8f)
            {
                // Attack Code Add
//                if (!target.data.attackCooltime[0])
                {
                    target.ChangeState(new MobPrepareAttack(0));
                }
            }
        }
    }

}
