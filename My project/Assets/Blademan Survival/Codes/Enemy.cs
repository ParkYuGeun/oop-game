using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float Health;
    public RuntimeAnimatorController[] animcon; //레벨에 따라 바꿀 스킨
    public Rigidbody2D target;                  //따라갈 Player
    
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

    void OnEnable()    //몹이 (재)사용될 시 초기화되는 설정
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isalive = true;
        col.enabled = true;    //콜라이더 끄는 법 = enabled
        rigid.simulated = true;    //리지드바디 끄는법 = simulated
        spriter.sortingOrder = 2;   //layer order 조정
        anim.SetBool("Dead", false);    //enemy 애니메이터 중 Dead속성 false로 
        Health = maxHealth;
        
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)   //플레이어의 islive가 false면 이동x
        {
            return;
        }
        if (!isalive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))  //자기가 죽어있거나 0번째 레이어 상태의 이름이Hit이라면 이동x
            return;
        //GetCurrentAnimatorStateInfo는 AnimatorStateInfo를 반환
        //AnimatorStateInfo는 IsName("이름"), normalizedTime, normalizedTime를 받아 State Name, 진행률, 애니메이션 길이를 받아올 수 있음



        Vector2 dirvec = target.position - rigid.position;  //플레이어캐릭터위치 - 몹위치 = 방향
        Vector2 nextvec = dirvec.normalized * speed * Time.deltaTime;   //몹이 받는 이동, time.deltatime은이전 프레임이 끝나고 다음 프레임까지 걸린 시간
        rigid.MovePosition(rigid.position+nextvec);                     //환경이 어떻든 프레임이 다르든 같은 속도로 이동하게 해줌 
        rigid.velocity = Vector2.zero;                      //몹 자체속도 제거

    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
        if (!isalive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;       //좀비 위치가 플레이어 위치보다 오른쪽에 있으면 좌우반전
    }



    public void Init(SpawnData data)    //생성될때 스펙정해주기
    {
        anim.runtimeAnimatorController = animcon[data.spritetype];  //레벨에 따른 몹 스킨
        speed = data.speed;
        maxHealth = data.health;
        Health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isalive)    //충돌한 오브젝트가 nullet이 아니거나 enemy가 죽어있을때는 아무것도없이 반환
            return;

        Health -= collision.GetComponent<Bullet>().damage;  //bullet이 있는 컴포넌트의 damage만큼 피가 깎임 
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

    void Dead()     //애니메이션에서 호출
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;     
        Vector3 dirVec = transform.position - playerPos;                //밀려나는 방향
        rigid.AddForce(dirVec.normalized*3,ForceMode2D.Impulse);        //ForceMode2D.force는 지속적인 힘, ForceMode2D.Impulse는 순간적인 힘                
    }                                                                   //AddForce(방향, 힘 종류)
}
