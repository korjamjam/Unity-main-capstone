using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //����
    public LayerMask targetLayer; // ���̾�
    public RaycastHit2D[] targets; // ��ĵ ��� �迭
    public Transform nearestTarget; // ���� ����� ��ǥ�� ���� ���� ����

    void FixedUpdate() // ��ĵ�� �ϱ� ���� ����
    {                               //ĳ���� ������ġ, ���� ������, ĳ���� ����, ĳ���ñ���, ��� ���̾� 
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // circlecastall ���� ���·� �����ϰڴ�.
        nearestTarget = GetNearest();
    }
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position; //�÷��̾� ��ġ
            Vector3 targetPos = target.transform.position; // ���� ĳ��Ʈ ��ġ �߿��� ���Ƿ� �ϳ����� Ÿ���� ��ġ
            float curDiff = Vector3.Distance(myPos, targetPos); // �� �ΰ��� �Ÿ�

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
