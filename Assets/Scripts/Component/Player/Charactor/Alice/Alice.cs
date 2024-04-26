using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Alice : Charactor
{

    public override void Attack()
    {
        attackPref = "AliceBaseAttackPref";
        base.Attack();

    }

    public override void Jump()
    {
        base.Jump();
    }
}
