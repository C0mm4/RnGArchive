using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bullet : Attack
{
    [SerializeField]
    protected Vector3 movPos = new Vector3();
    [SerializeField]
    protected ContactFilter2D contactFilter;
    [SerializeField]
    protected Rigidbody2D body;
    public float spd;

    public Animator animator;
    public override void CreateHandler(int dmg, Vector2 dir, AtkType t)
    {
        base.CreateHandler(dmg, dir, t);
        movPos = dir;

        Vector2 referenceVector = Vector2.right;
        float angle = Vector2.Angle(referenceVector, movPos);
        float sign = Mathf.Sign(Vector3.Cross(referenceVector, movPos).z);
        float signedAngle = angle * sign;

        transform.rotation = Quaternion.Euler(0, 0, signedAngle);

        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

        body = GetComponent<Rigidbody2D>();
        
        body.isKinematic = true;
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        RaycastHit2D[] hits = new RaycastHit2D[16];
        if(body != null)
        {
            var cnt = body.Cast(movPos, contactFilter, hits, spd * Time.deltaTime + 0.05f);


            for (int i = 0; i < cnt; i++)
            {
                if (hits[i].collider.CompareTag("Wall"))
                {
                    Destroy();
                    return;
                }
                if (hits[i].collider.CompareTag("Enemy"))
                {
                    EnterEnemy(hits[i].collider.GetComponent<Mob>());
                    Destroy();
                    return;
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
/*
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
*/
}
