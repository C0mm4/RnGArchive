using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportObj : Obj
{
    protected bool isAttack = false;

    public virtual void Attack()
    {

    }

    public override void Alarm0()
    {
        base.Alarm0();
        isAttack = false;
    }
}
