using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance; //바로 메모리로 이동
    public Player player;
    public PoolManager pool;

    public float GameTime;
    public float maxGameTime = 2*10f;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        GameTime += Time.deltaTime;

        if (GameTime > maxGameTime) {
            GameTime = maxGameTime;
           
        }

    }
}
