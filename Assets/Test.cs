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

        // ���� ���Ϳ� ��ǥ������ ���� ���� ���� ������ ����մϴ�.
        digrees= Vector3.Angle(sawDir, directionToTarget);

        // ������ ����մϴ�.
    }

}
