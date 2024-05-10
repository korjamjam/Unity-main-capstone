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
    public int[] nextExp = { 5, 10, 20, 50, 70, 100, 140, 200, 300, 500 };//���� ����ġ / public ���� ó�� ���� ����
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
        gameTime += Time.deltaTime; // ���������� �ð��� ��� ����

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
