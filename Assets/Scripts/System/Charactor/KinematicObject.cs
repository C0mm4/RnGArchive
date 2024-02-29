using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class KinematicObject : Obj
{
    /// <summary>
    /// The minimum normal (dot product) considered suitable for the entity sit on.
    /// </summary>
    public float minGroundNormalY = .45f;

    /// <summary>
    /// A custom gravity coefficient applied to this entity.
    /// </summary>
    [SerializeField]
    public float gravityModifier = 1f;

    /// <summary>
    /// The current velocity of the entity.
    /// </summary>
    public Vector2 velocity;
    protected Vector2 moveAccel;

    /// <summary>
    /// Is the entity currently sitting on a surface?
    /// </summary>
    /// <value></value>
    [SerializeField]
    public bool IsGrounded;

    public bool canMove = true;
    public bool isForceMoving;

    [SerializeField]
    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected Rigidbody2D body;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    public Animator animator;
    public string currentAnimation;

    public Vector3 moveTargetPos;

    public Vector2 _sawDir;
    public Vector2 sawDir {  get { return _sawDir; } set {  _sawDir = value; } }

    /// <summary>
    /// Bounce the objects velocity in a direction.
    /// </summary>
    /// <param name="dir"></param>
    public void Bounce(Vector2 dir)
    {
        velocity.y = dir.y;
        velocity.x = dir.x;
    }

    /// <summary>
    /// Teleport to some position.
    /// </summary>
    /// <param name="position"></param>
    public void Teleport(Vector3 position)
    {
        body.position = position;
        velocity *= 0;
        body.velocity *= 0;
    }

    protected virtual void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;
    }

    protected virtual void OnDisable()
    {
        body.isKinematic = false;
        
    }

    protected virtual void Start()
    {

    }

    public override void OnCreate()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        sawDir = new Vector2(1, 0);
        canMove = true;
        isForceMoving = false;
    }


    public override void Step()
    {
        base.Step();
        targetVelocity = Vector2.zero;
        ComputeVelocity();

    }

    protected virtual void ComputeVelocity()
    {

    }

    public virtual void FlipX(bool isFlip)
    {
        try
        {
            GetComponent<SpriteRenderer>().flipX = isFlip;

        }
        catch
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false; 
        }
    }

    public override void BeforeStep()
    {
        //if already falling, fall faster than the jump speed, otherwise use normal gravity.
        if (velocity.y < 0)
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            IsGrounded = false;
        }
        else
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;



        velocity.x = targetVelocity.x;

        if(sawDir.x < 0)
        {
            FlipX(true);
        }
        else
        {
            FlipX(false);
        }

        var deltaPosition = velocity * Time.deltaTime;  
        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        var move = moveAlongGround * deltaPosition.x;


        // Compute X Axis Move
        PerformMovement(move, false);

        // Change x, y in move vector
        move = Vector2.up * deltaPosition.y;

        // Compute Y Axis Move
        PerformMovement(move, true);

    }

    protected void PerformMovement(Vector2 move, bool yMovement)
    {
        var distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            //check if we hit anything in current direction of travel
            var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            for (var i = 0; i < count; i++)
            {
                CheckCollision(hitBuffer[i], yMovement);

                //remove shellDistance from actual move distance.
                if (hitBuffer[i].collider.tag == "Wall")
                {
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
                if (hitBuffer[i].collider.tag == "Enemy")
                {
                    if (hitBuffer[i].normal.y == 1)
                    {
                        var modifiedDistance = hitBuffer[i].distance - shellRadius;
                        distance = modifiedDistance < distance ? modifiedDistance : distance;
                    }
                }
            }
        }
        body.position += move.normalized * distance;
    }

    public virtual void CheckCollision(RaycastHit2D rh, bool yMovement)
    {
        var currentNormal = rh.normal;
        bool isCorrection = false;
        if (currentNormal.y >= minGroundNormalY)
        {
            IsGrounded = true;
            groundNormal = currentNormal;
            currentNormal.x = 0;
        }
        if (IsGrounded)
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
                {
                    // Collision with side walls
                    if(currentNormal.y == 0 && Mathf.Abs(currentNormal.x) > 0f)
                    {
                        // if on the Ground, up to staris correction
                        if (IsGrounded)
                        {
                            RaycastHit2D sideHit = Physics2D.Raycast(rh.point + GameManager.tileOffset * 1.1f * Vector2.up,
                                -currentNormal, GameManager.tileOffset.x / 2);
                            if(sideHit.collider == null)
                            {
                                body.position += GameManager.tileOffset * Vector2.up * 1.1f;
                                Debug.Log("Up to Stair");
                                isCorrection = true;
                            }
                        }
                    }
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
            if(!IsGrounded)
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

    public virtual void AnimationPlay(string clip, float spd = 1)
    {
        if(clip != currentAnimation)
        {
            if(System.Array.Exists(animator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
            {
                currentAnimation = clip;
                animator.speed = spd;
                animator.Play(clip);
            }
            else
            {
                Debug.Log($"Can't Find Clip : {clip}");
            }
        }
    }

    public async Task InDoor(Door door, Vector3 endPos = default)
    {
        
        isForceMoving = true;
        List<SpriteRenderer> renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach (var renderer in renderers)
        {
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        moveTargetPos = door.transform.position;


        await AwaitMoveToPosition(() => (Mathf.Abs(transform.position.x - moveTargetPos.x)) >= 0.05f);
        foreach (var renderer in renderers)
        {
            renderer.maskInteraction = SpriteMaskInteraction.None;
        }
        if(endPos != default)
        {
            moveTargetPos = endPos;

            await AwaitMoveToPosition(() => (Mathf.Abs(transform.position.x - moveTargetPos.x)) >= 0.05f);

            canMove = false;
        }
        else
        {
            canMove = false;
        }

        isForceMoving = false;
    }

    public async Task OutDoor(Door door)
    {
        isForceMoving = true;
        var dist = Vector2.Dot(transform.position - door.transform.position, door.InDir);
        Debug.Log(dist);
        // If IncorrectPosition move to correct position
        if (dist > 0)
        {
            moveTargetPos = door.transform.position;
            await AwaitMoveToPosition(() => Vector2.Dot(transform.position - door.transform.position, door.InDir) > 0);
        }

        moveTargetPos = door.transform.position;
        moveTargetPos.x += door.InDir.x * 1.5f * door.transform.localScale.x;
        Debug.Log(moveTargetPos);

        await AwaitMoveToPosition(() => (Mathf.Abs(transform.position.x - door.transform.position.x)) >= 0.05f);
        List<SpriteRenderer> renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach (var renderer in renderers)
        {
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        await AwaitMoveToPosition(() => (Mathf.Abs(transform.position.x - moveTargetPos.x)) >= 0.05f);
        isForceMoving = false;
    }

    private async Task AwaitMoveToPosition(Func<bool> condition)
    {
        while (condition())
        {
            await Task.Yield();
        }
    }

}