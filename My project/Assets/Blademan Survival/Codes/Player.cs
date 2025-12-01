using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;


    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();  
        spriter = GetComponent<SpriteRenderer>(); 
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {  
        if(!GameManager.Instance.isLive)
            return;
        inputVec.x = Input.GetAxis("Horizontal");   //�Է°��� ���� ���Ͱ� ����
        inputVec.y = Input.GetAxis("Vertical");
        //GetAxisRaw = ���� �������� ��ǲ
    }

    void FixedUpdate()
    {
        if(!GameManager.Instance.isLive)
            return;
        Vector2 nextVec = inputVec.normalized*speed*Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position+nextVec); //inputvec�� ĳ���Ϳ� ��������
    }

    void LateUpdate()
    {
        if(!GameManager.Instance.isLive)
            return;
        anim.SetFloat("speed",inputVec.magnitude);  //.magnitude ������ ��������
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if(GameManager.Instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            // 플레이어 사망 애니메이션이 없음 여기서 버그 발생
            anim.SetTrigger("Dead");
        }
    }
}
