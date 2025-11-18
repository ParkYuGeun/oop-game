using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    RectTransform rect;

     void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

     void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);    //월드좌표 -> 스크린좌표로 변환
    }
}
