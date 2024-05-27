using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDrone : AIModel
{
    RaycastHit2D[] hits = new RaycastHit2D[8];

    public override void Step()
    {
        currentState = target.currentState;

        StateControl();

        if (!target.isForceMoving)
        {
            if(currentState == "MobIdle" || currentState == "MobMove" || currentState == "MobJumpFinish" || currentState == "MobFalling")
            {
                target.SetTargetPosition(player.transform.position);
                if (target.GetPlayerDistance() <= 2f)
                {
                    target.canMove = false;
//                    if (!target.data.attackCooltime[0])
                        target.ChangeState(new MobPrepareAttack(0));
                }
                else
                {
                    target.canMove = true;
                }
            }

        }
    }

    public override void StateControl()
    {
        int cnt = target.body.Cast(Vector2.down, hits);


        for (int i = 0; i < cnt; ++i)
        {
            if (hits[i].normal.y == 1 && hits[i].collider.CompareTag("Wall"))
            {
                // Too close bottom wall flying up
                if (hits[i].distance < 0.5f)
                {
                    target.gravityModifier = -0.3f;
                    

                }
                // Hovering Distance
                else if (hits[i].distance < 0.6f)
                {
                    target.gravityModifier = 0f;
                    target.velocity = new Vector2(target.velocity.x, 0f);

                }
                // Too far bottom wall down
                else
                {
                    target.gravityModifier = 0.3f;

                }
            }
        }

        switch (currentState)
        {
            case "MobIdle":
                if (target.canMove)
                {
                    if (target.gravityModifier > 0f)
                    {
                        target.ChangeState(new MobFalling());
                    }
                    else if (target.gravityModifier < 0f)
                    {
                        target.ChangeState(new MobJumpFinish());
                    }
                    else if(Mathf.Abs(target.velocity.x) > 0.01f)
                    {
                        target.ChangeState(new MobMove());
                    }
                }
                break;
            case "MobMove":
                if (Mathf.Abs(target.velocity.x) < 0.01f)
                {
                    target.ChangeState(new MobIdle());
                }
                else
                {
                    if (target.gravityModifier > 0f)
                    {
                        target.ChangeState(new MobFalling());
                    }
                    else if (target.gravityModifier < 0f)
                    {
                        target.ChangeState(new MobJumpFinish());
                    }
                }
                break;
            case "MobJumpFinish":
                if (!target.canMove)
                {
                    target.ChangeState(new MobIdle());
                }
                break;
            case "MobFalling":
                if (!target.canMove)
                {
                    target.ChangeState(new MobIdle());
                }
                break;

        }
    }
}
