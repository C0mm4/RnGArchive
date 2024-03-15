using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttackObj : Obj
{
    public Mob shooter;

    public bool isSetData;

    public override void OnCreate()
    {
        base.OnCreate();
        GetComponent<Collider2D>().isTrigger = true;
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        if (isSetData)
        {
            if(shooter == null)
            {
                GameManager.Destroy(gameObject);
            }
        }
    }

    public virtual void SetData(Mob mob)
    {
        shooter = mob;
        isSetData = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().GetDmg(gameObject);
        }
    }

    public virtual void EndAttackState()
    {

    }

}
