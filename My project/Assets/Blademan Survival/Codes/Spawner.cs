using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] SpawnPoint;
    public SpawnData[] SpawnData;
    float timer;
    int level;
    public float levelTime;


    private void Awake()
    {
        SpawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.Instance.maxGameTime / SpawnData.Length;
    }

    void Update()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
        timer += Time.deltaTime;    //10�ʴ� ���� 1�� �ö󰡴� ���
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / levelTime), SpawnData.Length-1);  //mathf.floortoint = int����ȯ(����) �ø��� celltoint

        if (timer > SpawnData[level].spawntime)
        {
            timer = 0;
            Spawn();            
        }

    }

    void Spawn()    //�ð����� spawn����, spawn�� poolmanager.get����
    {
        GameObject enemy =  GameManager.Instance.pool.Get(0);
        enemy.transform.position = SpawnPoint[Random.Range(1,SpawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(SpawnData[level]);
    }
}

//�����ý���
[System.Serializable]
public class SpawnData
{
    public float spawntime;
    public int spritetype;  //�� ����
    public int health;
    public float speed;
}