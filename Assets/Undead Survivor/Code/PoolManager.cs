using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ��������� ������ ���� * ������ 2���� ����Ʈ�� 2�� *
    public GameObject[] prefabs;

    // Ǯ ����ϴ� ����Ʈ��
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; //pool��� �迭 �ʱ�ȭ

        for (int index = 0; index < pools.Length; index++) // �� �ȿ� �ִ� ����Ʈ ��ȸ�ϸ� �ʱ�ȭ
        {
            pools[index] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;
        // ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ ��) ���� ������Ʈ ����
           // �߰��ϸ� select ������ �Ҵ�
        foreach(GameObject item in pools[index])//�迭,����Ʈ���� �����͸� ���������� �����ϴ� �ݺ���
        {
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item; 
                select.SetActive(true);
                break;
            }
        }
        // ��ã������?
        if(!select)
        {
            // ���Ӱ� �����ؼ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select); //pool�� ���
        }
        return select;
    }
}
