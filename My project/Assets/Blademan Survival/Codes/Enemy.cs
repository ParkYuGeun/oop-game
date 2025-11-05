using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float Health;
    public RuntimeAnimatorController[] animcon; //레벨에 따라 바꿀 스킨
    public Rigidbody2D target;

    bool isalive;  

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        if (!isalive)
            return; 

        Vector2 dirvec = target.position - rigid.position;  //플레이어캐릭터위치 - 몹위치 = 방향
        Vector2 nextvec = dirvec.normalized * speed * Time.deltaTime;   //몹이 받는 이동
        rigid.MovePosition(rigid.position+nextvec);
        rigid.velocity = Vector2.zero;  //몹 자체속도 제거

    }

    private void LateUpdate()
    {
        if (!isalive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;   
    }

     void OnEnable()    //몹이 재사용될 시 초기화되는 설정
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isalive = true;
        Health = maxHealth;

    }

    public void Init(SpawnData data)    //생성될때 스펙정해주기
    {
        anim.runtimeAnimatorController = animcon[data.spritetype];
        speed = data.speed;
        maxHealth = data.health;
        Health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        Health -= collision.GetComponent<Bullet>().damage;

        if (Health > 0) { }

        else
        {
            Dead();
        }

 
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
