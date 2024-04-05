using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);//deltaTime 한 프레임이 소비한 시간

                break;
            default:
                break;
        }
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150; // "-" 해야 시계 방향으로 돌음
                Batch();
                
                break;
            default:
                break;
        }
    }

    void Batch() //count마다 poolmanage에 있는 두번째 prefab을 가져옴
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
            bullet.parent = transform;
            bullet.GetComponent<Bullet>().Init(damage, -1);//-1 is Infinity Per.
        }
    }
}
