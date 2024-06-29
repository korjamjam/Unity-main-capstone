using System.Collections;
using System.Collections.Generic;
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

    public void Init(float damage, int per, Vector3 dir) // ������, ���밪, ����
    {
        this.damage = damage;
        this.per = per;

        if (per > -1) //-1���� ũ�� ���Ÿ�, ū�Ϳ� ���ؼ� �ӵ� ����
        {
            rigid.velocity = dir * 10f; //�Ѿ� ���ư��� �ӵ�
        }
    }
    void OnTriggerEnter2D(Collider2D collision) //����
    {
        if (!collision.CompareTag("Enemy") || per == -1)//|| = or, per�� -1�̸� �Ʒ� ���� ������ �ʿ� x 
            return;

        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero; //��Ȱ��ȭ ������ ���� �ӵ� �ʱ�ȭ
            gameObject.SetActive(false);// ���밪�� �ٴٰ� -1 �Ǹ� ��Ȱ��ȭ
        }
    }
}
