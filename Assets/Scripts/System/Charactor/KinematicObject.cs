using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicObject : Obj
{
    /// <summary>
    /// The minimum normal considered suitable for the entity sit on
    /// </summary>
    public float minGroundNormalY = .45f;


    /// <summary>
    /// A custom gravity coefficient applied to this entity.
    /// </summary>
    [SerializeField]
    public float gravityModifier = 1f;

    /// <summary>
    /// Is the eentity currently sitting on a surface?
    /// </summary>
    public bool isGrounded;
    [SerializeField]
    protected Vector2 groundNormal;

    private Rigidbody2D body;
    protected ContactFilter2D contactFilter;
    [SerializeField]
    protected RaycastHit2D[] hitBufferX = new RaycastHit2D[16];
    protected RaycastHit2D[] hitBufferY = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    public Vector2 velocity;

    protected virtual void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;
    }


    public virtual void OnDisable()
    {
        body.isKinematic = false;
        body = null;
    }

    public override void OnCreate()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        groundNormal = Vector2.up;

    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        
        // Add gravity force
        velocity = gravityModifier * Physics2D.gravity * Time.deltaTime * 50;
        isGrounded = false;

        var deltaPosition = velocity * Time.deltaTime;
        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        var move = moveAlongGround * deltaPosition;
        PerformMovement(deltaPosition);
/*
        // Compute X Axis Move
        PerformMovement(move, false);

        // Change x, y in move vector
        move = Vector2.up * deltaPosition.y;

        // Compute Y Axis Move
        PerformMovement(move, true);*/
    }

    protected void PerformMovement(Vector2 deltaPos)
    {
        var Xdistance = deltaPos.x;
        var Ydistance = deltaPos.y;

        var rayX = Vector2.right * deltaPos;
        var rayY = Vector2.down * deltaPos;
        if(deltaPos.magnitude > minMoveDistance)
        {
            hitBufferX = new RaycastHit2D[16];
            hitBufferY = new RaycastHit2D[16];

            var xcnt = body.Cast(rayX, contactFilter, hitBufferX, Xdistance + shellRadius);
            var ycnt = body.Cast(rayY, contactFilter, hitBufferY, Ydistance + shellRadius);
            Debug.Log(xcnt + " " + ycnt);
            for (var i = 0; i < xcnt; i++)
            {
               Debug.Log(Time.time + " : " + hitBufferX[i].point);
            }
            for (var i = 0; i < ycnt; i++)
            {
                Debug.Log(Time.time + " : " + hitBufferY[i].point);
                var modifiedDistance = hitBufferY[i].distance - shellRadius;
                Debug.Log(modifiedDistance + " , " + Ydistance);
                Ydistance = modifiedDistance < Ydistance ? modifiedDistance : Ydistance;
                Debug.Log(Ydistance);
                if(modifiedDistance <= -shellRadius)
                {
                    Debug.Log("A");
                    Ydistance = GetComponent<BoxCollider2D>().size.y - (body.position.y - hitBufferY[i].point.y) * 2 ;
                }
            }

        }
        body.position += Vector2.up * Ydistance;
    }

/*    protected void PerformMovement(Vector2 move, bool yMovement)
    {
        var distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            //check if we hit anything in current direction of travel
            var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            for (var i = 0; i < count; i++)
            {
                Debug.Log(Time.time + " : " + hitBuffer[i].point);
                CheckCollision(hitBuffer[i], yMovement);

                //remove shellDistance from actual move distance.
                if (hitBuffer[i].collider.tag == "Wall" || hitBuffer[i].collider.tag == "Enemy")
                {
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
        }
        body.position += move.normalized * distance;
    }*/

    public virtual void CheckCollision(RaycastHit2D rh, bool yMovement)
    {
        var currentNormal = rh.normal;
        bool isCorrection = false;
        if (currentNormal.y >= minGroundNormalY)
        {
            isGrounded = true;
            groundNormal = currentNormal;
            currentNormal.x = 0;
        }
        if (isGrounded)
        {
            //how much of our velocity aligns with surface normal?
            var projection = Vector2.Dot(velocity, currentNormal);
            if (projection < 0)
            {
                //slower velocity if moving against the normal (up a hill).
                velocity -= projection * currentNormal;
            }
        }
        if (rh.collider.tag == "Wall")
        {
            // Check for X Axis Collision
            if (!yMovement)
            {
                if (velocity.y <= 0)
                {/*
                    // Collision with side walls
                    if (currentNormal.y == 0 && Mathf.Abs(currentNormal.x) > 0f)
                    {
                        // if on the Ground, up to staris correction
                        if (isGrounded)
                        {
                            RaycastHit2D sideHit = Physics2D.Raycast(rh.point + GameManager.tileOffset * 1.1f * Vector2.up,
                                -currentNormal, GameManager.tileOffset.x / 2);
                            if (sideHit.collider == null)
                            {
                                body.position += GameManager.tileOffset * Vector2.up * 1.1f;
                                Debug.Log("Up to Stair");
                                isCorrection = true;
                            }
                        }
                    }*/
                }
            }
            else
            {
                // Hit Rooftop Fall down
                if (currentNormal.y < 0)
                {
                    velocity.y = -0.5f;
                }
            }
            if (!isGrounded)
            {
                //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                if (!isCorrection)
                {
                    velocity.x *= 0;

                }
            }
            else
            {
                if (!yMovement && !isCorrection)
                {
                    velocity.x *= 0;
                }
            }
        }


    }

}
