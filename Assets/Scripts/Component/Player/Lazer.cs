using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lazer : Bullet
{

    public List<Mob> attackedMobs = new();
    public override void CreateHandler(int dmg, Vector2 dir, AtkType t)
    {
        base.CreateHandler(dmg, dir, t);
        if (dir.y == 0)
        {
            movPos.x = 15 * dir.x;
        }
        movPos.y = 15 * dir.y;
    }


    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Wall")
        {
            GameManager.Destroy(gameObject);

        }

        if(collider.tag == "Enemy")
        {
            if (!attackedMobs.Contains(collider.GetComponent<Mob>()))
            {
                collider.GetComponent<Mob>().GetDMG(dmg, type);
                attackedMobs.Add(collider.GetComponent<Mob>());
            }
        }
    }

}
