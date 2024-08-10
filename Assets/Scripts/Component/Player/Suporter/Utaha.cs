using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utaha : SupportObj
{
    float detectDistance = 5f;
    public GameObject head;
    GameObject target;

    public GameObject Muzzle;

    public float headDigrees;
    public float targetDigrees;

    public override void OnCreate()
    {
        base.OnCreate();
    }

    public override void Step()
    {
        spawnT += Time.deltaTime;
        base.Step();
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = detectDistance;
        target = null;
        headDigrees = head.transform.rotation.z;
        foreach (GameObject obs in mobs)
        {
            float dist = (obs.transform.position - transform.position).magnitude;
            if(dist < distance)
            {
                var d = PointDigrees(obs);
                if (d >= -10 && d <= 24)
                {
                    target = obs;
                    if(d >= 0)
                    {
                        targetDigrees = d;
                    }
                    else
                    {
                        targetDigrees = 0f;
                    }
                    distance = dist;

                }
            }
        }

        if(target == null)
        {
            targetDigrees = 0f;
        }

        var deltaDegrees = Time.deltaTime * 1f;
        if(targetDigrees < headDigrees)
        {
            deltaDegrees *= 1f;
        }

        head.transform.rotation = Quaternion.Euler(0, 0, headDigrees + deltaDegrees);


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
        GameObject fireEffect = GameManager.InstantiateAsync("UtahaMuzzleFire", Muzzle.transform.position, head.transform.rotation);
        fireEffect.transform.SetParent(Muzzle.transform, true);
        
    }

    public float PointDigrees(GameObject targetPoint)
    {
        float ret;
        Vector3 directionToTarget = targetPoint.transform.position - transform.position;

        // 기준 벡터와 목표점으로 가는 벡터 간의 각도를 계산합니다.
        ret = Vector3.Angle(sawDir, directionToTarget);
        return ret;
    }
}
