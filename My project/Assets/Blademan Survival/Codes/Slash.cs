using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public Animator anim;   //원본 애니메이션

    float clipLength;   //애니메이션 길이

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (anim == null)
        {
            Debug.Log("애니메이션 연결x");
            return;
        }
        
        clipLength = GetClipLength(anim);
        Debug.Log(clipLength);  //애니메이션 길이 받아오는지 확인

        if (clipLength > 0)
        {
            StartCoroutine(DisableAfterTime(clipLength));
        }
        else
        {
            Debug.LogWarning("현재 애니메이션 클립 길이를 가져올 수 없습니다. 기본값 1초 후 비활성화됩니다.", this);
            // 클립 길이를 못 찾으면 안전하게 1초 후 비활성화
            StartCoroutine(DisableAfterTime(1f));
        }
    }

    private float GetClipLength(Animator anim)
    {
        if (anim.runtimeAnimatorController == null)
        {
            Debug.Log("애니메이션 없음");
            return 0f;
        }

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);  
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if(clipInfo.Length>0)
            return clipInfo[0].clip.length; //성공 시

        Debug.Log("클립 길이 못구함");
        return 0;
    }

    private IEnumerator DisableAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }


}
