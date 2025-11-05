using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;     //원거리 인식할 범위
    public LayerMask targetLayer;   //적 인식 레이어
    public RaycastHit2D[] target;   //레이저 포인터로 쐈을 때, 어디에 맞았는지, 얼마나 멀리 있는지, 뭘 맞췄는지
    public Transform nearestTarget; //가장 가까운 tf위치

    private void FixedUpdate()
    {
        target = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        //매개변수//1.위치2.원의반지름3.방향4.길이5.대상
        nearestTarget = getNearest();

    }

    Transform getNearest()  //RaycastHit2D[]중 가장 가까운거
    {
        Transform result = null;
        float diff = 100f;
        foreach (RaycastHit2D target in target)
        {
            Vector3 myPosition = transform.position;
            Vector3 targetPosition = target.transform.position;

            float curDiff = Vector3.Distance(myPosition, targetPosition);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
