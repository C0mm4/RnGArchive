using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Pattern : Obj
{
    public Mob mob;

    public bool isSetData;

    public bool isActivate;

    public override void OnCreate()
    {
        base.OnCreate();
        isSetData = false;
        isActivate = false;
    }

    public virtual void SetData(Mob mob)
    {
        this.mob = mob;
        isSetData = true;
    }

    

    public abstract Task Action();
}
