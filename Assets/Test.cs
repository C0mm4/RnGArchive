using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector2 sawDir = Vector2.right;
    public GameObject targetPoint;

    public float digrees;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Update()
    {
        Vector3 directionToTarget = targetPoint.transform.position - transform.position;
        /*        float ret;

                // 기준 벡터와 목표점으로 가는 벡터 간의 각도를 계산합니다.
                ret = Vector3.Angle(sawDir, directionToTarget);
        //        return ret;

                float angle = Vector3.Angle(sawDir, directionToTarget);

                // 외적을 계산하여 시계 방향 여부를 판단합니다.
                float sign = Mathf.Sign(Vector3.Dot(sawDir, Vector3.Cross(sawDir, directionToTarget)));

                // 시계 방향 각도를 반환합니다.
                float signedAngle = angle * sign;*/

        // 두 벡터 사이의 기본 각도를 구합니다.
        float angle = Vector3.Angle(sawDir, directionToTarget);

        // 외적을 계산하여 시계 방향 여부를 판단합니다.
        float sign = Mathf.Sign(Vector3.Dot(sawDir, Vector3.Cross(Vector3.back, directionToTarget)));

        // 시계 방향 각도를 반환합니다.
        float signedAngle = angle * sign;

        if (sawDir.x < 0)
        {
            signedAngle = -signedAngle;
        }

        digrees = signedAngle;

    }

}
