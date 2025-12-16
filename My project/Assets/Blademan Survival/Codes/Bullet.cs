using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; //관통

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1)
        {
            rigid.velocity = dir*15f;

        }
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)    //무기가 근접(per=-1)이거나 부딫친게 적이 아닐때)
            return;
        per--;
        if (per == -1) { 
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
