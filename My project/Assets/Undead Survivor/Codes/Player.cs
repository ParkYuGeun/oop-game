using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 3.0f;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();  
        spriter = GetComponent<SpriteRenderer>(); 
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        inputVec.x = Input.GetAxis("Horizontal");   //입력값에 따른 벡터값 증가
        inputVec.y = Input.GetAxis("Vertical");
        //GetAxisRaw = 딱딱 끊어지는 인풋
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized*speed*Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position+nextVec); //inputvec을 캐릭터에 때려박음
    }

    void LateUpdate()
    {
        anim.SetFloat("speed",inputVec.magnitude);  //.magnitude 벡터의 순수길이
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
