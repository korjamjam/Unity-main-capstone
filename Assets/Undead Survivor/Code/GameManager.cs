using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 5, 10, 20, 50, 70, 100, 140, 200, 300, 500 };//레벨 경험치 / public 사용시 처음 값만 적용
    [Header("# GameObject")]
    public PoolManager pool;
    public Player player;
    
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        gameTime += Time.deltaTime; // 한프레임의 시간을 계속 더함

        if(gameTime> maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
    public void Getexp()
    {
        exp++;

        if(exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
