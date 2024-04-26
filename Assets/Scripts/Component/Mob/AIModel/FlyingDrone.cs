using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDrone : AIModel
{
    public override void Step()
    {
        currentState = target.currentState;


        RaycastHit2D[] hits = new RaycastHit2D[8];
        int cnt = target.body.Cast(Vector2.down, hits);

        for(int i = 0; i < cnt; ++i)
        {
            if (hits[i].normal.y == 1 && hits[i].collider.CompareTag("Wall"))
            {
                // Too close bottom wall flying up
                if (hits[i].distance < 2f)
                {
                    target.gravityModifier = -0.3f;
                }
                // Hovering Distance
                else if (hits[i].distance  < 2.5f)
                {
                    target.gravityModifier = 0f;
                    target.body.velocity = new Vector2(target.body.velocity.x, 0f);
                }
                // Too far bottom wall down
                else
                {
                    target.gravityModifier = 0.3f;
                }
            }
        }

        if (!target.isForceMoving)
        {
            if(currentState == "Idle" || currentState == "MobMove")
            {
                target.SetTargetPosition(player.transform.position);
            }
            if(target.GetPlayerDistance() <= 4f)
            {
                if (!target.data.attackCooltime[0])
                    target.ChangeState(new MobAttack(0));
            }
        }
    }
}
