using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float Health;
    public RuntimeAnimatorController[] animcon; //������ ���� �ٲ� ��Ų
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
        if(!GameManager.Instance.isLive)
            return;
        if (!isalive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))  //0��° ���̾� ������ �̸���Hot�̶��
            return; 

        Vector2 dirvec = target.position - rigid.position;  //�÷��̾�ĳ������ġ - ����ġ = ����
        Vector2 nextvec = dirvec.normalized * speed * Time.deltaTime;   //���� �޴� �̵�
        rigid.MovePosition(rigid.position+nextvec);
        rigid.velocity = Vector2.zero;  //�� ��ü�ӵ� ����

    }

    private void LateUpdate()
    {
        if(!GameManager.Instance.isLive)
            return;
        if (!isalive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;   
    }

     void OnEnable()    //���� ����� �� �ʱ�ȭ�Ǵ� ����
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isalive = true;
        col.enabled = true;    //�ݶ��̴� ���� �� = enabled
        rigid.simulated = true;    //������ٵ� ���¹� = simulated
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        Health = maxHealth;

    }

    public void Init(SpawnData data)    //�����ɶ� ���������ֱ�
    {
        anim.runtimeAnimatorController = animcon[data.spritetype];
        speed = data.speed;
        maxHealth = data.health;
        Health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isalive)    //�浹�� ������Ʈ�� nullet�� �ƴϰų� enemy�� �׾��������� �ƹ��͵����� ��ȯ
            return;

        Health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (Health > 0) {
            anim.SetTrigger("Hit"); //�ִϸ��̼� ��Ʈ�ѷ� Ʈ���� �۵�

        }

        else
        {
            isalive = false;
            col.enabled = false;    //�ݶ��̴� ���� �� = enabled
            rigid.simulated = false;    //������ٵ� ���¹� = simulated
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
