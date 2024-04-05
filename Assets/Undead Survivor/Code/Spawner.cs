using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime; // 한프레임의 시간을 계속 더함
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length -1);// 소수점 아래 버리고 int형으로 바꾸는 함수

        if(timer > spawnData[level].spawnTime) //타이머가 일정 시간 값에 도달하면 소환
        {
            timer = 0;
            Spawn();
        }
    }
    
    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position; //자식 오브젝트만 선택되도록 랜덤 시작은 1부터
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// 직렬화 : 개채를 저장 혹은 전송하기 위해 변환, 인스팩터에서 초기화 가능
[System.Serializable]
public class SpawnData
{
    // 스프라이트 타입, 소환시간, 체력, 속도
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}