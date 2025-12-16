using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //�ٷ� �޸𸮷� �̵�
    [Header("# Game Control")]
    public bool isLive;
    public float GameTime;
    public float maxGameTime = 2 * 10f;
    public bool isLive;

    [Header("# Player Info")]
    public int playerId;
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
    public Transform uiJoy;
    public GameObject EnemyCleaner;

    void Awake()
    {
        Instance = this;            //�ʼ�
         Application.targetFrameRate = 60;
    }

    public void GameStart(int index)        //��ư�� �Ҵ�
    {
        playerId = index;
        health = maxHealth;
        uiLevelUp.select(index);    //ù��°���� ����
        player.gameObject.SetActive(true);
        resume();

        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SELECT);
    }

    public void GameOver()      //Player ����� �Ҵ�
    {
        StartCoroutine(GameOverRoutine());
    }

    public void GameVictory()   //gameTime�ٵǸ� ȣ��
    {
        StartCoroutine(GameVictoryRoutine());
    }

    public void GameRetry()     //result ��ư�� �Ҵ�
    {
        SceneManager.LoadScene(0);  //index��� "�� �̸�"���ε� ȣ�� ����
    }

    public void GameQuit()     //result ��ư�� �Ҵ�
    {
        Application.Quit();
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
        if (exp == nextExp[Mathf.Min(level,nextExp.Length-1)])  //���� �ִ�� 10������ nextLevel ������ 9���̴�
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
        uiJoy.localScale = Vector3.zero;
    }

    public void resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);  //��� �ִϸ��̼� ��ٸ��� �ð�
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.LOSE);
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        EnemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.WIN);
    }
}


