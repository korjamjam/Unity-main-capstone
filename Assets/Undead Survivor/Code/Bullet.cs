using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
       rigid = GetComponent<Rigidbody2D>(); 
    }

    public void Init(float damage, int per, Vector3 dir) // 데미지, 관통값, 방향
    {
        this.damage = damage;
        this.per = per;

        if(per > -1) //-1보다 크면 원거리, 큰것에 대해서 속도 적용
        {
            rigid.velocity = dir * 10f; //총알 날아가는 속도
        }
    }

    void OnTriggerEnter2D(Collider2D collision) //관통
    {
        if (!collision.CompareTag("Enemy") || per == -1)//|| = or, per가 -1이면 아래 로직 실행할 필요 x 
            return;
        
        per--;

        if(per == -1)
        {
            rigid.velocity = Vector2.zero; //비활성화 이전에 물리 속도 초기화
            gameObject.SetActive(false);// 관통값이 줄다가 -1 되면 비활성화
        }
    }
    /*
    void Update()
    {
        Dead();
    }
    void Dead() //원거리 무기가 플레이어와 일정거리 멀어지면 사라짐
    {
        Transform target = GameManager.Instance.player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        if (dir > 20f)
            this.gameObject.SetActive(false);
    }*/
}
