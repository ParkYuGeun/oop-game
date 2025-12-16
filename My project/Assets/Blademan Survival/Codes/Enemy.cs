using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float Health;
    public RuntimeAnimatorController[] animcon; //������ ���� �ٲ� ��Ų
    public Rigidbody2D target;                  //���� Player
    
    bool isalive;
    bool dotDamage;

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

    void OnEnable()    //���� (��)���� �� �ʱ�ȭ�Ǵ� ����
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isalive = true;
        col.enabled = true;    //�ݶ��̴� ���� �� = enabled
        rigid.simulated = true;    //������ٵ� ���¹� = simulated
        spriter.sortingOrder = 2;   //layer order ����
        anim.SetBool("Dead", false);    //enemy �ִϸ����� �� Dead�Ӽ� false�� 
        Health = maxHealth;
        dotDamage = false;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)   //�÷��̾��� islive�� false�� �̵�x
        {
            return;
        }
        if (!isalive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))  //�ڱⰡ �׾��ְų� 0��° ���̾� ������ �̸���Hit�̶�� �̵�x
            return;
        //GetCurrentAnimatorStateInfo�� AnimatorStateInfo�� ��ȯ
        //AnimatorStateInfo�� IsName("�̸�"), normalizedTime, normalizedTime�� �޾� State Name, �����, �ִϸ��̼� ���̸� �޾ƿ� �� ����



        Vector2 dirvec = target.position - rigid.position;  //�÷��̾�ĳ������ġ - ����ġ = ����
        Vector2 nextvec = dirvec.normalized * speed * Time.deltaTime;   //���� �޴� �̵�, time.deltatime������ �������� ������ ���� �����ӱ��� �ɸ� �ð�
        rigid.MovePosition(rigid.position+nextvec);                     //ȯ���� ��� �������� �ٸ��� ���� �ӵ��� �̵��ϰ� ���� 
        rigid.velocity = Vector2.zero;                      //�� ��ü�ӵ� ����

        if (dotDamage) {
            Health = Health - 1 * Time.deltaTime;
        }

    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
        if (!isalive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;       //���� ��ġ�� �÷��̾� ��ġ���� �����ʿ� ������ �¿����
    }



    public void Init(SpawnData data)    //�����ɶ� ���������ֱ�
    {
        anim.runtimeAnimatorController = animcon[data.spritetype];  //������ ���� �� ��Ų
        speed = data.speed;
        maxHealth = data.health;
        Health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isalive)    //�浹�� ������Ʈ�� nullet�� �ƴϰų� enemy�� �׾��������� �ƹ��͵����� ��ȯ
            return;

        Health -= collision.GetComponent<Bullet>().damage;  //bullet�� �ִ� ������Ʈ�� damage��ŭ �ǰ� ���� 
        StartCoroutine(KnockBack());

        if (Health > 0) {
            anim.SetTrigger("Hit"); //�ִϸ��̼� ��Ʈ�ѷ� Ʈ���� �۵�
            AudioManager.Instance.PlaySfx(AudioManager.SFX.HIT0);
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

            if (GameManager.Instance.isLive)
                AudioManager.Instance.PlaySfx(AudioManager.SFX.DEAD);
        }

        if (collision.TryGetComponent(out Fire fire)) {
            dotDamage = true;
        }

        if (collision.TryGetComponent(out Water water))
        {
            GameManager.Instance.health += 10;
        }
    }

    void Dead()     //�ִϸ��̼ǿ��� ȣ��
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;     
        Vector3 dirVec = transform.position - playerPos;                //�з����� ����
        rigid.AddForce(dirVec.normalized*3,ForceMode2D.Impulse);        //ForceMode2D.force�� �������� ��, ForceMode2D.Impulse�� �������� ��                
    }                                                                   //AddForce(����, �� ����)
}
