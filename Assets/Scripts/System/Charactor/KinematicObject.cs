using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Unity.VisualScripting;
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
    public Vector2 moveAccel;

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
    protected RaycastHit2D[] xHitBuffer = new RaycastHit2D[16];
    protected RaycastHit2D[] yHitBuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    public Animator animator;
    public string currentAnimation;

    public Vector3 moveTargetPos;

    public Vector2 _sawDir;
    public Vector2 sawDir {  get { return _sawDir; } set {  _sawDir = value; } }

    protected bool isJump;

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
        groundNormal = Vector3.up;
        isJump = false;
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
        }
        else
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        }



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
        if (isJump)
        {
            Debug.Log(deltaPosition.y);
        }
        PerformMovement(deltaPosition);


    }

    protected void PerformMovement(Vector2 move)
    {
        var distance = move.magnitude;
        var xDistance = move.x;
        var yDistance = move.y;
        xHitBuffer = new RaycastHit2D[16];
        yHitBuffer = new RaycastHit2D[16];
        if(distance > minMoveDistance)
        {
            var xMove = new Vector2(move.x, 0f);
            var xCount = body.Cast(xMove, contactFilter, xHitBuffer, xDistance + shellRadius);
            List<GameObject> collisionList = new List<GameObject>();
            List<KeyValuePair<GameObject, RaycastHit2D>> applyHitsX = new List<KeyValuePair<GameObject, RaycastHit2D>>();
            List<KeyValuePair<GameObject, RaycastHit2D>> applyHitsY = new List<KeyValuePair<GameObject, RaycastHit2D>>();
            for(var i = 0; i < xCount; i++)
            {
                if (xHitBuffer[i].collider != null)
                {
                    collisionList.Add(xHitBuffer[i].collider.gameObject);
                    applyHitsX.Add(new KeyValuePair<GameObject, RaycastHit2D>(xHitBuffer[i].collider.gameObject, xHitBuffer[i]));
                }
            }
            var yMove = new Vector2(0f, move.y);
            var yCount = body.Cast(yMove, contactFilter, yHitBuffer, yDistance + shellRadius);
            for(var i = 0; i <= yCount; i++)
            {
                if (yHitBuffer[i].collider != null)
                {
                    if (collisionList.Contains(yHitBuffer[i].collider.gameObject))
                    {
                        KeyValuePair<GameObject, RaycastHit2D> kv = applyHitsX.Find((item) => item.Key == yHitBuffer[i].collider.gameObject);
                        RaycastHit2D xRay = kv.Value;
                        try
                        {
                            Vector3 dir = new Vector3(xRay.point.x, xRay.point.y) - (kv.Key.transform.position + new Vector3(kv.Key.GetComponent<Collider2D>().offset.x, kv.Key.GetComponent<Collider2D>().offset.y));

                            float xBound = kv.Key.GetComponent<Collider2D>().bounds.extents.x - Mathf.Abs(dir.x);
                            float yBound = kv.Key.GetComponent<Collider2D>().bounds.extents.y - Mathf.Abs(dir.y);

                            if (yBound < xBound)
                            {
                                applyHitsX.Remove(kv);
                                applyHitsY.Add(new KeyValuePair<GameObject, RaycastHit2D>(yHitBuffer[i].collider.gameObject, yHitBuffer[i]));
                            }

                        }
                        catch
                        {
                            collisionList.Add(yHitBuffer[i].collider.gameObject);
                            applyHitsY.Add(new KeyValuePair<GameObject, RaycastHit2D>(yHitBuffer[i].collider.gameObject, yHitBuffer[i]));
                        }
                    }
                    else
                    {
                        collisionList.Add(yHitBuffer[i].collider.gameObject);
                        applyHitsY.Add(new KeyValuePair<GameObject, RaycastHit2D>(yHitBuffer[i].collider.gameObject, yHitBuffer[i]));
                    }

                }
            }

            foreach(KeyValuePair<GameObject, RaycastHit2D> hit in applyHitsX)
            {
                moveAccel.x *= 0f;
                var modifiedDistance = hit.Value.distance - shellRadius;
                if (modifiedDistance == -shellRadius)
                {
                    float collisionDistance = transform.position.x - hit.Value.point.x;
                    float boundy = GetComponent<Collider2D>().bounds.extents.x;
                    float offset = GetComponent<Collider2D>().offset.x;
                    if(collisionDistance < 0)
                    {
                        xDistance = -(boundy - Mathf.Abs(collisionDistance + offset)) * 2f;
                    }
                    else
                    {
                        xDistance = (boundy - Mathf.Abs(collisionDistance + offset)) * 2f;

                    }
                }
                else
                {
                    xDistance = modifiedDistance < xDistance ? modifiedDistance : xDistance;
                }
            }


            body.position += Vector2.right * xDistance;


            IsGrounded = false;
            foreach (KeyValuePair<GameObject, RaycastHit2D> hit in applyHitsY)
            {
                if(hit.Value.normal.y > minGroundNormalY && !isJump)
                {
                    groundNormal = hit.Value.normal;
                    IsGrounded = true;
                    velocity.y = 0;
                }
                var modifiedDistance = hit.Value.distance - shellRadius;
                if(move.y > 0)
                {
                    yDistance = modifiedDistance < yDistance ? yDistance : modifiedDistance;
                }
                else
                {
                    if (modifiedDistance == -shellRadius)
                    {
                        float collisionDistance = transform.position.y - hit.Value.point.y;
                        float boundy = GetComponent<Collider2D>().bounds.extents.y;
                        float offset = GetComponent<Collider2D>().offset.y;
                        if (collisionDistance < 0)
                        {
                            yDistance = -(boundy - Mathf.Abs(collisionDistance + offset)) * 2f;
                        }
                        else
                        {
                            yDistance = (boundy - Mathf.Abs(collisionDistance + offset)) * 2f;

                        }
                        Debug.Log(yDistance);
                    }
                    else
                    {
                        yDistance = modifiedDistance < yDistance ? modifiedDistance : yDistance;
                    }
                }
            }



            body.position += Vector2.up * yDistance;

        }

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
/*            // Check for X Axis Collision
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
            else*/
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

            canMove = true;
        }
        else
        {
            canMove = true;
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