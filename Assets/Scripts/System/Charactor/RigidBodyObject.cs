using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RigidBodyObject : Obj
{
    [SerializeField]
    public float gravityModifier = 1f;

    public Rigidbody2D body;

    public bool isGrounded;

    public Vector2 sawDir;

    public bool canMove;

    public bool isForceMoving;

    public Vector3 targetMovePos;

    public void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.gravityScale = gravityModifier;
        body.isKinematic = false;
        body.bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnDisable()
    {
        body = null;
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        isGrounded = false;
        FlipX();
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);

    }

    public virtual void OnCollisionStay2D(Collision2D other)
    {
        CheckCollision(other);
        
    }



    public virtual void CheckCollision(Collider2D obj)
    {

    }

    public virtual void CheckCollision(Collision2D obj)
    {
        foreach(ContactPoint2D contact in obj.contacts)
        {
            if(contact.normal.y > 0f)
            {
                isGrounded = true;
            }
        }
    }

    public virtual void FlipX()
    {
        if(sawDir.x < 0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
