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
    Collider2D col;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
         
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        col = GetComponent<Collider2D>();

    }

    void FixedUpdate()
    {
        if (!isalive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))  //0번째 레이어 상태의 이름이Hot이라면
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
        col.enabled = true;    //콜라이더 끄는 법 = enabled
        rigid.simulated = true;    //리지드바디 끄는법 = simulated
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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
        if (!collision.CompareTag("Bullet") || !isalive)    //충돌한 오브젝트가 nullet이 아니거나 enemy가 죽어있을때는 아무것도없이 반환
            return;

        Health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (Health > 0) {
            anim.SetTrigger("Hit"); //애니메이션 컨트롤러 트리거 작동

        }

        else
        {
            isalive = false;
            col.enabled = false;    //콜라이더 끄는 법 = enabled
            rigid.simulated = false;    //리지드바디 끄는법 = simulated
            spriter.sortingOrder = 1;
            anim.SetBool("Dead",true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();
            
        }

 
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized*3,ForceMode2D.Impulse);
    }
}
