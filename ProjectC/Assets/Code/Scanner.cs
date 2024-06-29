using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //범위
    public LayerMask targetLayer; // 레이어
    public RaycastHit2D[] targets; // 스캔 결과 배열
    public Transform nearestTarget; // 가장 가까운 목표를 담을 변수 선언

    void FixedUpdate() // 스캔을 하기 위한 로직
    {                               //캐스팅 시작위치, 원의 반지름, 캐스팅 방향, 캐스팅길이, 대상 레이어 
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // circlecastall 원형 형태로 검증하겠다.
        nearestTarget = GetNearest();
    }
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position; //플레이어 위치
            Vector3 targetPos = target.transform.position; // 레이 캐스트 위치 중에서 임의로 하나꺼낸 타겟의 위치
            float curDiff = Vector3.Distance(myPos, targetPos); // 위 두개의 거리

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
