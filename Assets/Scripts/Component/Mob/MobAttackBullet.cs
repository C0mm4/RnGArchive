using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttackBullet : MobAttackObj
{
    public Vector3 dir;
    public float spd;

    public override void CreateHandler()
    {
        base.CreateHandler();
        dir = GameManager.player.transform.position - shooter.transform.position;
        Vector2 right = Vector2.right;

        float dirmagni = dir.magnitude;
        float rightmagni = right.magnitude;
        float dot = Vector2.Dot(dir, right);

        float angle = Mathf.Acos(dot / (dirmagni * rightmagni)) * Mathf.Rad2Deg;
        if (dir.y < 0)
        {
            angle = 360 - angle;

        }
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        transform.position += spd * Time.deltaTime * dir;
        
    }

}
