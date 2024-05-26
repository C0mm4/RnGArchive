using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTest : Obj
{
    public float gravityModifier = 1f;

    public bool isGrounded;

    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    public Rigidbody2D body;

    public Vector2 velocity;
    private Vector2 additionalVelocty;

    public Vector2 groundNormal;

    public Vector2 sawDir;

    public bool canMove;
    public bool isMove;
    public bool isForceMoving;

    public Vector3 targetMovePos;

    public bool isLanding;

    protected virtual void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;

        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = false;
    }

    public override void BeforeStep()
    {
        if(velocity.y < 0)
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        }
        else
        {
            velocity += Physics2D.gravity * Time.deltaTime;
        }
        // Add External Force
        velocity += additionalVelocty;

        base.BeforeStep();

        isGrounded = false;

        // Calculate Movement
        var deltaPos = velocity * Time.deltaTime;
        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        var move = moveAlongGround * deltaPos.x;
        
        PerformMovement(move, false);

        move = Vector2.up * deltaPos.y;

        PerformMovement(move, true);

        // Reset External Force
        velocity -= additionalVelocty;

        // Decay External Force
        DecayAdditionalVelocity();

        FlipX();
    }

    public virtual void PerformMovement(Vector2 dir, bool yMovement)
    {
        var distance = dir.magnitude;
        if (distance > minMoveDistance)
        {
            // Check hit buffer
            var cnt = body.Cast(dir, contactFilter, hitBuffer, distance + shellRadius);

            for (int i = 0; i < cnt; i++)
            {
                if (hitBuffer[i].collider.isTrigger)
                {
                    continue;
                }

                var currentNormal = hitBuffer[i].normal;
                // Check Bottom hit
                if (currentNormal.y > 0.5f)
                {
                    isGrounded = true;
                    if (yMovement)
                    {
                        velocity.y = 0f;
                        groundNormal = currentNormal;
                    }
                }
                // Check Head hit
                if (currentNormal.y < -0.5f)
                {
                    velocity.y = Mathf.Min(velocity.y, 0);
                }
               
                if (isGrounded)
                {
/*                    var projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0)
                    {
                        if (hitBuffer[i].collider.CompareTag("Wall"))
                            velocity -= projection * currentNormal;
                    }*/
                }
               
                var modifiedDistance = hitBuffer[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
            var moveDistance = dir.normalized * distance;
            body.position += moveDistance;
        }
    }

    public void AddForce(Vector2 force)
    {
        additionalVelocty += force;
    }

    private void DecayAdditionalVelocity()
    {
        additionalVelocty.x *= (1 - 0.1f);
        if(additionalVelocty.y > 0)
        {
            additionalVelocty += gravityModifier * Physics2D.gravity * Time.deltaTime;
        }
        else
        {
            additionalVelocty.y = 0f;
        }
        if(additionalVelocty.magnitude <= 0.01f)
        {
            additionalVelocty = Vector2.zero;
        }
    }

    public virtual void FlipX()
    {
        if (sawDir.x > 0f)
        {
            transform.localRotation = new Quaternion(0, 0, 0, 0);

        }
        else
        {
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }

    }

}
