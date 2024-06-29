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
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime; // ���������� �ð��� ��� ����
        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 10f);// �Ҽ��� �Ʒ� ������ int������ �ٲٴ� �Լ�

        if (timer > spawnData[level].spawnTime) //Ÿ�̸Ӱ� ���� �ð� ���� �����ϸ� ��ȯ
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; //�ڽ� ������Ʈ�� ���õǵ��� ���� ������ 1����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// ����ȭ : ��ä�� ���� Ȥ�� �����ϱ� ���� ��ȯ, �ν����Ϳ��� �ʱ�ȭ ����
[System.Serializable]
public class SpawnData
{
    // ��������Ʈ Ÿ��, ��ȯ�ð�, ü��, �ӵ�
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}