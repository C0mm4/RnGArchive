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

                // ���� ���Ϳ� ��ǥ������ ���� ���� ���� ������ ����մϴ�.
                ret = Vector3.Angle(sawDir, directionToTarget);
        //        return ret;

                float angle = Vector3.Angle(sawDir, directionToTarget);

                // ������ ����Ͽ� �ð� ���� ���θ� �Ǵ��մϴ�.
                float sign = Mathf.Sign(Vector3.Dot(sawDir, Vector3.Cross(sawDir, directionToTarget)));

                // �ð� ���� ������ ��ȯ�մϴ�.
                float signedAngle = angle * sign;*/

        // �� ���� ������ �⺻ ������ ���մϴ�.
        float angle = Vector3.Angle(sawDir, directionToTarget);

        // ������ ����Ͽ� �ð� ���� ���θ� �Ǵ��մϴ�.
        float sign = Mathf.Sign(Vector3.Dot(sawDir, Vector3.Cross(Vector3.back, directionToTarget)));

        // �ð� ���� ������ ��ȯ�մϴ�.
        float signedAngle = angle * sign;

        if (sawDir.x < 0)
        {
            signedAngle = -signedAngle;
        }

        digrees = signedAngle;

    }

}
