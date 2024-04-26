using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Obj
{
    public int dmg;
    public Vector2 dir;
    public AtkType type;

    public override void OnCreate()
    {
        base.OnCreate();
        GetComponent<Collider2D>().isTrigger = true;
        transform.tag = "Attack";
    }

    public virtual void CreateHandler(int dmg, Vector2 dir, AtkType type)
    {
        this.dmg = dmg;
        this.dir = dir;
        this.type = type;
    }
/*
    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {

            collider.GetComponent<Mob>().GetDMG(dmg, type);
        }
    }*/


    public virtual void EnterEnemy(Mob target)
    {
        target.GetDMG(dmg, type);
    }
}
