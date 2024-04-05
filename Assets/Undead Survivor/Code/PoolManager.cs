using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩들을 보관할 변수 * 변수가 2개면 리스트도 2개 *
    public GameObject[] prefabs;

    // 풀 담당하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; //pool담는 배열 초기화

        for (int index = 0; index < pools.Length; index++) // 그 안에 있는 리스트 순회하며 초기화
        {
            pools[index] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;
        // 선택한 풀의 놀고 있는 (비활성화 된) 게임 오브젝트 접근
           // 발견하면 select 변수에 할당
        foreach(GameObject item in pools[index])//배열,리스트를의 데이터를 순차적으로 접근하는 반복문
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item; 
                select.SetActive(true);
                break;
            }
        }
        // 못찾았으면?
        if(!select)
        {
            // 새롭게 생성해서 select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select); //pool에 등록
        }
        return select;
    }
}
