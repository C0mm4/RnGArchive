using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : Obj
{
    public Image fillBar;
    public Mob target;

    public override void AfterStep()
    {
        base.AfterStep();
        if (target != null)
        {
            fillBar.fillAmount = (float)target.status.currentHP / (float)target.status.maxHP;
        }
        else
        {
            GameManager.Destroy(gameObject);
        }
    }
    

}
