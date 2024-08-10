using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector2 sawDir = Vector2.right;
    public GameObject targetPoint;

    public float digrees;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Update()
    {
        Vector3 directionToTarget = targetPoint.transform.position - transform.position;

        // 기준 벡터와 목표점으로 가는 벡터 간의 각도를 계산합니다.
        digrees= Vector3.Angle(sawDir, directionToTarget);

        // 각도를 출력합니다.
    }

}
