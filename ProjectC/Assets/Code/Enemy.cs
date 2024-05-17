using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPractice : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    // Update is called once per frame
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate() // �������� �̵��� ���� �Լ�
    {
        if (!isLive)//������� ���� �������� �Լ�
            return;

        Vector2 dirVec = target.position - rigid.position; // ���� ���� = Ÿ����ġ - ���� ��ġ
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //����Ű�� ������ ���ܳ� ���� ��ġ�� �� 
        rigid.MovePosition(rigid.position + nextVec); //������ġ + ���� ��ġ
        rigid.velocity = Vector2.zero; //���� �ӵ��� �̵��� ������ ���� �ʵ��ϼӵ� ����
    }
    void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
    }

}