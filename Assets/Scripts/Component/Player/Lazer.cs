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

    public override void BeforeStep()
    {
        RaycastHit2D[] hits = new RaycastHit2D[16];
        var cnt = body.Cast(movPos, contactFilter, hits, (spd * Time.deltaTime) * 1.1f + 0.05f);

        Debug.Log(hits.Length);

        for(int i = 0; i < cnt; i++)
        {
            if (hits[i].collider.CompareTag("Wall"))
            {
                Destroy();
                break;
            }
            if (hits[i].collider.CompareTag("Enemy"))
            {
                if (!attackedMobs.Contains(hits[i].collider.GetComponent<Mob>()))
                {
                    EnterEnemy(hits[i].collider.GetComponent<Mob>());
                    attackedMobs.Add(hits[i].collider.GetComponent<Mob>());
                }
            }
        }

        transform.position += movPos * Time.deltaTime * spd;

        Vector3 viewportPosition = GameManager.CameraManager.maincamera.WorldToViewportPoint(transform.position);

        // Check if the object is outside the camera's viewport
        if (viewportPosition.x < 0 || viewportPosition.x > 1 ||
            viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            // Object is outside the camera's viewport, destroy it
            Destroy();
        }


    }
}
