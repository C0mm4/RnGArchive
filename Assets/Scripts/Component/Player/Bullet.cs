using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bullet : Attack
{
    protected Vector3 movPos = new Vector3();

    public override void CreateHandler(int dmg, Vector2 dir, AtkType t)
    {
        base.CreateHandler(dmg, dir, t);
        if(dir.y == 0)
        {
            movPos.x = 10 * dir.x;
        }
        movPos.y = 10 * dir.y;
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        transform.position += movPos * Time.deltaTime;

        Vector3 viewportPosition = GameManager.CameraManager.maincamera.WorldToViewportPoint(transform.position);

        // Check if the object is outside the camera's viewport
        if (viewportPosition.x < 0 || viewportPosition.x > 1 ||
            viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            // Object is outside the camera's viewport, destroy it
            GameManager.Destroy(gameObject);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if(collider.tag == "Wall")
        {
            GameManager.Destroy(gameObject);
        }
        if(collider.tag == "Enemy")
        {
            GameManager.Destroy(gameObject);
        }
    }

}
