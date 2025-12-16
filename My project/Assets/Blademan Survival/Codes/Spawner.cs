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
        timer += Time.deltaTime;    //10초당 레벨 1씩 올라가는 기능
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / levelTime), SpawnData.Length-1);  //mathf.floortoint = int형변환(버림) 올림은 celltoint

        if (timer > SpawnData[level].spawntime)
        {
            timer = 0;
            Spawn();            
        }

    }

    void Spawn()    //시간마다 spawn실행, spawn은 poolmanager.get실행
    {
        GameObject enemy =  GameManager.Instance.pool.Get(0);
        enemy.transform.position = SpawnPoint[Random.Range(1,SpawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(SpawnData[level]);
    }
}

//레벨시스템
[System.Serializable]
public class SpawnData
{
    public float spawntime;
    public int spritetype;  //몹 종류
    public int health;
    public float speed;
}