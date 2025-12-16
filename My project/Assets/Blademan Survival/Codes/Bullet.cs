using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;    //데미지
    public int per; //관통

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    //물리
    }

    public void Init(float damage, int per, Vector3 dir)    
    {
        this.damage = damage;
        this.per = per;

        if (per > -1)       //원거리일경우(관통이 양수) 투사체 속도
        {
            rigid.velocity = dir*15f;   

        }
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1 )    //무기가 근접(per=-1)이거나 부딫친게 적이 아닐때 아무것도없이 반환
            return;
        per--;
        if (per == -1) {                                    //관통수치가 한번씩 내려가고 -1이 되면 투사체 속도 없애고 삭제
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

     void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area") || per ==-1)
            return;
        gameObject.SetActive(false) ;
    }
}
