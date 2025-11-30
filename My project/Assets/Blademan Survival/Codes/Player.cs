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
        if (!GameManager.Instance.isLive) {
            return;
        }   
        inputVec.x = Input.GetAxis("Horizontal");   //입력값에 따른 벡터값 증가
        inputVec.y = Input.GetAxis("Vertical");
        //GetAxisRaw = 딱딱 끊어지는 인풋
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
        Vector2 nextVec = inputVec.normalized*speed*Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position+nextVec); //inputvec을 캐릭터에 때려박음
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
        anim.SetFloat("speed",inputVec.magnitude);  //.magnitude 벡터의 순수길이
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

     void OnCollisionStay2D(Collision2D collision)   //충돌하고있을시
    {
        if (!GameManager.Instance.isLive || collision.gameObject.CompareTag("Building"))
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if (GameManager.Instance.health < 0) {
            for (int index = 2; index < transform.childCount; index++) {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("dead");
            GameManager.Instance.GameOver();
        }
    }
}
