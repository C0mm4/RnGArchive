using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bullet : Attack
{
    public override void CreateHandler(int dmg, Vector2 dir, AtkType t)
    {
        base.CreateHandler(dmg, dir, t);
        if(dir.y == 0)
        {
            moveAccel.x = 10 * dir.x;
        }
        velocity.y = 10 * dir.y;
    }

    public override void CheckCollision(RaycastHit2D rh, bool yMovement)
    {
        base.CheckCollision(rh, yMovement);
        if(rh.collider.tag == "Wall")
        {
            GameManager.Destroy(gameObject);
        }
        if(rh.collider.tag == "Enemy")
        {
            GameManager.Destroy(gameObject);
        }
    }
}
