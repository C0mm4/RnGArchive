using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Boss : Mob
{
    public GameObject HPBar;
    public override void CreateHPBar()
    {
        HPBar = GameManager.InstantiateAsync("BossHPBar");
        Func.SetRectTransform(HPBar);
        HPBar.GetComponent<EnemyHPBar>().target = this;
    }


}
