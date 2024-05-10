using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttackHitBox : MobAttackObj
{
    public override void EndAttackState()
    {
        base.EndAttackState();
        Destroy();
    }
}
