using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utaha : SupportObj
{
    float detectDistance = 5f;
    GameObject target;
    float spawnT;

    public override void OnCreate()
    {
        base.OnCreate();
        spawnT = 0f;
    }

    public override void Step()
    {
        spawnT += Time.deltaTime;
        base.Step();
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = detectDistance;
        target = null;
        foreach (GameObject obs in mobs)
        {
            float dist = (obs.transform.position - transform.position).magnitude;
            if(dist < distance)
            {
                target = obs;
                distance = dist;
            }
        }

        if (!isAttack)
        {
            if (target != null)
            {
                if (GameManager.GetUIState() == UIState.InPlay)
                {
                    isAttack = true;
                    Attack();
                    SetAlarm(0, 0.5f);

                }
            }
        }
        if(spawnT > 5f)
        {
            Destroy();
        }

    }

    public override void Attack()
    {
        base.Attack();
        GameObject go = GameManager.InstantiateAsync("UtahaBullet", transform.position);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.CreateHandler(1,target.transform.position - transform.position, AtkType.Piercing);
    }
}
