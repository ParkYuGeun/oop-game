using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance; //바로 메모리로 이동
    public Player player;

    private void Awake()
    {
        Instance = this;
    }
}
