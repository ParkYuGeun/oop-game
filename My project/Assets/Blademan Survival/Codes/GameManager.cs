using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //바로 메모리로 이동
    [Header("# Game Control")]
    public float GameTime;
    public float maxGameTime = 2 * 10f;
    public bool isLive;

    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject EnemyCleaner;


     void Awake()
    {
        Instance = this;
    }

     public void GameStart()
    {
        health = maxHealth;
        uiLevelUp.select(0);    //첫번째무기 선택
        resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);  //씬 이름으로도 호출 가능
    }

    void Update()
    {
        if (!isLive) {
            return;
        }

        GameTime += Time.deltaTime;

        if (GameTime > maxGameTime)
        {
            GameTime = maxGameTime;
            GameVictory();
        }

    }

    public void GetExp()
    {   
        if(!isLive)
            return;
        exp++;
        if (exp == nextExp[Mathf.Min(level,nextExp.Length-1)])  //레벨 최대는 10이지만 nextLevel 개수는 9개이다
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);  //사망 애니메이션 기다리는 시간
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        stop();
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        EnemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        stop();
    }
}


