using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;  //이 코드 내부에서 식별하는 update
    public int prefabId;//pool에서 받는 prefab번호
    public float damage;
    public int count;   //원거리 관통 , 근거리는 -1
    public float speed;

    float timer;
    Player player;

     void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void Start()
    {
        Init();
    }

    public void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            case 1:
                timer += Time.deltaTime;
                if(timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
            case 2:
                timer += Time.deltaTime;
                if(timer > speed+1)
                {
                    Slash();
                    timer = 0;
                }

                break;

        }

        if (Input.GetButtonDown("Jump"))
        {
            Levelup(20, 1);
        }
    }

    //테스트레벨업
    public void Levelup(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }

    void Init()
    {
        switch (id)
        {
            case 0:
                speed = -150;
                Batch();
                break;

            case 1:
                speed = 1f;
                break;
            default:
                speed = 0.5f;
                 break;
        }
    }

    void Batch()
    {
        for (int i = 0; i < count; i++){
            Transform bullet;          

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform;  //poolmanager에서 부모 변경
            }
           

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage,-1,Vector3.zero);  //-1은 무한관통
            
        }

    }

    void Fire() {
        if (!player.scanner.nearestTarget)
        {
            return;
        }

        Vector3 targetPos =  player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage,count,dir);
    }

    void Slash()
    {
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.parent = transform;
        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        //위치
        //각도
        //한번만 실행
    }

}
