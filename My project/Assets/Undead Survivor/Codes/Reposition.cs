using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area"))   //충돌한 오브젝트의 태그가 Area가 아니면 반환
            return;
        Vector3 PlayerPosition = GameManager.Instance.player.transform.position;
        Vector3 MyPosition = transform.position;
        float diffx = Mathf.Abs(PlayerPosition.x - MyPosition.x);
        float diffy = Mathf.Abs(PlayerPosition.y - MyPosition.y);

        Vector3 PlayerDir = GameManager.Instance.player.inputVec;
        float dirx = PlayerDir.x < 0 ? -1 : 1;
        float diry = PlayerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirx * 40);
                }
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * diry * 40);
                }
                break;

            case "Enemy":

                break;
        }
    }
}
