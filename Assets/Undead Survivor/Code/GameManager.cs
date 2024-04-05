using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime;
    public float maxGameTime = 2 * 10f;

    public PoolManager pool;
    public Player player;
    
    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        gameTime += Time.deltaTime; // 한프레임의 시간을 계속 더함

        if(gameTime> maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
}
