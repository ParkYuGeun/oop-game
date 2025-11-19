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
    Vector3 tempDir = Vector3.right;

    void Awake()
    {
        player = GameManager.Instance.player;
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
                // 타이머가 공격 주기(speed + 1)를 넘으면 활성화 실행
                if (timer > speed)
                {                   
                    SlashOn();
                    timer = 0;
                }
                break;
            default:
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

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);   //player오브젝트가 가지고있는 자식 중 ApplyGear을 가지고있는 모든 컴포넌트들이 실행함
    }

    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;    //부모 플레이어로 관리
        transform.localPosition = Vector3.zero; //생성위치 플레이어위치로 초기화
        //Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++) { 
            if(data.projecTile == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;

            case 1:
                speed = 0.4f;
                break;
            case 2:
                speed = 2;
                ;
                break;
            default:
                break;
        }
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);   //player오브젝트가 가지고있는 자식 중 ApplyGear을 가지고있는 모든 컴포넌트들이 실행함
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

    void SlashOn()
    {
        // 1. 오브젝트 풀에서 공격 오브젝트(슬래시 이펙트)를 가져옴
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        

        // 2. 부모를 무기 오브젝트(이 스크립트가 붙은 오브젝트)로 설정하여 Hierarchy 정리
        bullet.parent = transform;

        // --- [새로 추가된 위치 및 회전 계산 로직] ---

        // 3. 플레이어의 입력 방향 벡터를 가져옴 (dir)
        Vector3 dir = GameManager.Instance.player.inputVec;
        dir = (dir.normalized) / 2;

        // 4. 입력이 없는 경우 기본 방향 설정 (예: 정면)
        
        if (dir != Vector3.zero)
        {         
            tempDir =dir; // 혹은 이전에 캐릭터가 바라보던 방향을 사용
        }

        if (dir == Vector3.zero)
        {
            dir = tempDir; // 혹은 이전에 캐릭터가 바라보던 방향을 사용
        }

        // 5. 위치 계산: 플레이어 위치에서 dir 방향으로 1.5f 떨어진 지점
        // (Weapon 오브젝트가 플레이어의 자식이라면, transform.position 대신 Vector3.zero를 사용해야 합니다.
        // 여기서는 Weapon이 플레이어의 위치에 있다고 가정하고 코드를 작성합니다.)

        // 이펙트가 플레이어의 위치에 생성되도록 설정
        bullet.position = transform.position;

        // 그 다음, dir 방향으로 1.5f 이동
        bullet.Translate(dir.normalized * 0.5f, Space.World);

        // 6. 회전 설정: 오브젝트의 '앞' 방향(Vector3.up을 가정)이 dir 방향을 향하도록 회전
        // 이 회전은 발사체의 로컬 Y축이 dir 벡터와 일치하도록 만듭니다.
        bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);

        // 7. 능력치 초기화
        // Bullet 스크립트의 Init 함수가 Vector3.zero를 받으므로, 이동 방향은 bullet 자체가 담당하게 됩니다.
        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
    }

}
