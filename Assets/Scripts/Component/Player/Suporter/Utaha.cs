using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utaha : SupportObj
{
    float detectDistance = 5f;
    public GameObject head;
    GameObject target;

    public GameObject Muzzle;
    public GameObject Joint;

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
        foreach (GameObject obs in mobs)
        {
            float dist = (obs.transform.position - transform.position).magnitude;
            if(dist < distance)
            {
                var d = PointDigrees(obs);
                if (d >= -10 && d <= 40)
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

        
        headDigrees = Mathf.MoveTowardsAngle(headDigrees, targetDigrees, 10f * Time.deltaTime);
        head.transform.localRotation = Quaternion.Euler(0, transform.rotation.y, headDigrees);

        
        if(Mathf.Abs(targetDigrees - headDigrees) < 3f)
        {
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
        }

    }

    public override void Attack()
    {
        base.Attack();
        GameObject go = GameManager.InstantiateAsync("UtahaBullet", Muzzle.transform.position);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.CreateHandler(1,target.transform.position - transform.position, AtkType.Piercing);
        GameObject fireEffect = GameManager.InstantiateAsync("UtahaMuzzleFire", Muzzle.transform.position, head.transform.rotation);
        fireEffect.transform.SetParent(Muzzle.transform, true);
        
    }

    public float PointDigrees(GameObject targetPoint)
    {
        Vector3 directionToTarget = targetPoint.transform.position - Joint.transform.position;

        // 두 벡터 사이의 기본 각도를 구합니다.
        float angle = Vector3.Angle(sawDir, directionToTarget);

        // 외적을 계산하여 시계 방향 여부를 판단합니다.
        float sign = Mathf.Sign(Vector3.Dot(sawDir, Vector3.Cross(Vector3.back, directionToTarget)));

        // 시계 방향 각도를 반환합니다.
        float signedAngle = angle * sign;

        if(sawDir.x < 0)
        {
            signedAngle = -signedAngle;
        }

        return signedAngle;

    }
}
