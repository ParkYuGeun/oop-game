using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area"))   //충돌한 오브젝트의 태그가 Area가 아니면 반환
            return;
        Vector3 PlayerPosition = GameManager.Instance.player.transform.position;
        Vector3 MyPosition = transform.position;

        switch (transform.tag)
        {
            case "Building":
                if (PlayerPosition.x > transform.position.x) {
                    transform.Translate(Vector3.right * 30);
                }
                else if (PlayerPosition.x < transform.position.x)
                {
                    transform.Translate(Vector3.left * 30);
                }
                break;

            case "Ground":
                float diffX = PlayerPosition.x - MyPosition.x;  //거리
                float diffY = PlayerPosition.y - MyPosition.y;
                float dirX = diffX < 0 ? -1 : 1;        //방향
                float dirY = diffY < 0 ? -1 : 1;        
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":      
                if (coll.enabled){
                    Vector3 distance = PlayerPosition - MyPosition;
                    Vector3 ran = new Vector3(Random.Range(-3,3), Random.Range(-3,3),0);
                    transform.Translate(ran + distance*2);
                }
                break;
        }
    }
}
