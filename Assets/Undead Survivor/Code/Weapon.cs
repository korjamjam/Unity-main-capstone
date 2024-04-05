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
                transform.Rotate(Vector3.back * speed * Time.deltaTime);//deltaTime �� �������� �Һ��� �ð�

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
                speed = 150; // "-" �ؾ� �ð� �������� ����
                Batch();
                
                break;
            default:
                break;
        }
    }

    void Batch() //count���� poolmanage�� �ִ� �ι�° prefab�� ������
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
            bullet.parent = transform;
            bullet.GetComponent<Bullet>().Init(damage, -1);//-1 is Infinity Per.
        }
    }
}
