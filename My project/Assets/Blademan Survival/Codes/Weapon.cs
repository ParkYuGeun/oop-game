using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public int id;  //�� �ڵ� ���ο��� �ĺ��ϴ� update
    public int prefabId;//pool���� �޴� prefab��ȣ
    public float damage;
    public int count;   //���Ÿ� ���� , �ٰŸ��� -1
    public float speed; //����
    public float initSpeed;

    float timer;    
    Player player;  
    Vector3 tempDir = Vector3.right;    
    Vector3 boxSize = Vector3.one;

    void Awake()
    {
        player = GameManager.Instance.player;
    }



    public void Update()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
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
            case 5:
            case 6:
            case 7:
                timer += Time.deltaTime;
                // Ÿ�̸Ӱ� ���� �ֱ�(speed + 1)�� ������ Ȱ��ȭ ����
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

    //������
    public void Levelup(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();

        if(id>=5)
            sizeUp(count);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);   //player������Ʈ�� �������ִ� �ڽ� �� ApplyGear�� �������ִ� ��� ������Ʈ���� ������
    }

    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon" + data.itemId;          //���ӿ�����Ʈ �̸�
        transform.parent = player.transform;    //�θ� �÷��̾�� ����
        transform.localPosition = Vector3.zero; //������ġ �÷��̾���ġ�� �ʱ�ȭ
        //Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        initSpeed = data.baseCount;
        //������Id�� poolmanager �����Ϳ� ���ؼ� Ȯ���ϰ� index�ޱ�
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
                speed = -150;
                Batch();
                break;

            case 1:
                speed = 0.4f;
                break;
            case 5:
            case 6:
            case 7:
                speed = initSpeed;
                break;
            default:
                break;
        }
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);   //player������Ʈ�� �������ִ� �ڽ� �� ApplyGear�� �������ִ� ��� ������Ʈ���� ������
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
                bullet.parent = transform;  //poolmanager���� �θ� ����
            }
           

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage,-1,Vector3.zero);  //-1�� ���Ѱ���
            
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
        AudioManager.Instance.PlaySfx(AudioManager.SFX.RANGE);
    }

    void SlashOn()
    {
        // 1. ������Ʈ Ǯ���� ���� ������Ʈ(������ ����Ʈ)�� ������
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        
        // 2. �θ� ���� ������Ʈ(�� ��ũ��Ʈ�� ���� ������Ʈ)�� �����Ͽ� Hierarchy ����
        bullet.parent = transform;
        bullet.localScale = boxSize;
        // --- [���� �߰��� ��ġ �� ȸ�� ��� ����] ---

        // 3. �÷��̾��� �Է� ���� ���͸� ������ (dir)
        Vector3 dir = GameManager.Instance.player.inputVec;
        dir = (dir.normalized) / 2;

        // 4. �Է��� ���� ��� �⺻ ���� ���� (��: ����)
        if (dir != Vector3.zero)
        {         
            tempDir =dir; // Ȥ�� ������ ĳ���Ͱ� �ٶ󺸴� ������ ���
        }

        if (dir == Vector3.zero)
        {
            dir = tempDir; // Ȥ�� ������ ĳ���Ͱ� �ٶ󺸴� ������ ���
        }

        // 5. ��ġ ���: �÷��̾� ��ġ���� dir �������� 1.5f ������ ����
        // (Weapon ������Ʈ�� �÷��̾��� �ڽ��̶��, transform.position ��� Vector3.zero�� ����ؾ� �մϴ�.
        // ���⼭�� Weapon�� �÷��̾��� ��ġ�� �ִٰ� �����ϰ� �ڵ带 �ۼ��մϴ�.)

        // ����Ʈ�� �÷��̾��� ��ġ�� �����ǵ��� ����
        bullet.position = transform.position;

        // �� ����, dir �������� 1.5f �̵�
        bullet.Translate(dir.normalized * 0.5f, Space.World);

        // 6. ȸ�� ����: ������Ʈ�� '��' ����(Vector3.up�� ����)�� dir ������ ���ϵ��� ȸ��
        // �� ȸ���� �߻�ü�� ���� Y���� dir ���Ϳ� ��ġ�ϵ��� ����ϴ�.
        bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);

        // 7. �ɷ�ġ �ʱ�ȭ
        // Bullet ��ũ��Ʈ�� Init �Լ��� Vector3.zero�� �����Ƿ�, �̵� ������ bullet ��ü�� ����ϰ� �˴ϴ�.
        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);

        AudioManager.Instance.PlaySfx(AudioManager.SFX.MELEE1);
    }


    void sizeUp(int index)
    {
        float scaleFactor = 1 + (index / 10f);
        boxSize = Vector3.one * scaleFactor;
    }
}
