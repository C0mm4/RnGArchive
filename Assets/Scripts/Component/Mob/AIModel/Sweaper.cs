using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class Sweaper : AIModel
{
    public override void Step()
    {
        currentState = target.currentState;
        StateControl();

        if (!target.isForceMoving)
        {
            if (canAccessPlayer)
            {
                Vector3 pos = default(Vector3);
                if(path.Count > 1)
                {
                    pos = GameManager.Stage.currentMap.mapTile.CellToWorld(new Vector3Int(path[1].x, path[1].y, 0));
                }
                else 
                {
                    pos = target.transform.position;
                }
                
                target.SetTargetPosition(pos);
            }
            if (currentState == "MobIdle" || currentState == "MobMove")
            {
                if (target.GetPlayerDistance(target.player) <= 0.3f * target.transform.localScale.x)
                {
                    if (!target.data.attackIsCool[0])
                        target.ChangeState(new MobPrepareAttack(0));
                }
            }
        }
/*
        if (!target.isForceMoving)
        {
            if(currentState == "MobIdle" || currentState == "MobMove")
            {
                target.SetTargetPosition(player.transform.position);
            }
            if (target.GetPlayerDistance(player) <= 0.8f)
            {
                // Attack Code Add
//                if (!target.data.attackCooltime[0])
                {
                    target.ChangeState(new MobPrepareAttack(0));
                }
            }
        }*/
    }

    public override void StateControl()
    {
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
                if (!target.isMove)
                {
                    target.ChangeState(new MobIdle());
                }
                break;
            case "MobJumpFinish":
                if(target.velocity.y <= 0)
                {
                    target.ChangeState(new MobFalling());
                }
                break;
            case "MobFalling":
                if (target.isGrounded)
                {
                    target.ChangeState(new MobLanding());
                }
                break;
        }
    }

}
