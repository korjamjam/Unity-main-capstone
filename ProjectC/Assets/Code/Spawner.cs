using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime; // ���������� �ð��� ��� ����

        if (timer > 0.2f) //Ÿ�̸Ӱ� ���� �ð� ���� �����ϸ� ��ȯ
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; //�ڽ� ������Ʈ�� ���õǵ��� ���� ������ 1����
    }
}