using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : KinematicObject
{
    public int dmg;

    public override void OnCreate()
    {
        base.OnCreate();
        gravityModifier = 0f;
        body = GetComponent<Rigidbody2D>();
        groundNormal = new Vector2(0, 1);
        transform.tag = "Attack";
    }

    public virtual void CreateHandler(int dmg, Vector2 dir)
    {
        this.dmg = dmg;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();
        targetVelocity.x = moveAccel.x;
    }

    public override void CheckCollision(RaycastHit2D rh, bool yMovement)
    {
        var currentNormal = rh.normal;
        if(rh.collider.tag == "Enemy")
        {
            rh.collider.GetComponent<Enemy>().GetDMG(dmg);
        }
    }
}
