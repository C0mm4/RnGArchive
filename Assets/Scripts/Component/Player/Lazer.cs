using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : Attack
{
    public override void CreateHandler(int dmg, Vector2 dir, AtkType t)
    {
        base.CreateHandler(dmg, dir, t);
        if (dir.y == 0)
        {
            moveAccel.x = 15 * dir.x;
        }
        velocity.y = 15 * dir.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
