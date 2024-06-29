using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 2��° input ���

public class Player1 : MonoBehaviour // ���� ���� ������ �ʿ��� �͵��� ���� Ŭ����
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    Rigidbody2D rigid;//���� ����
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //������Ʈ���� ������Ʈ�� �������� �Լ�
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    void Update() // 1��° input ���
    {
        if (!GameManager.Instance.isLive)
            return;
            
        inputVec.x = Input.GetAxisRaw("Horizontal");//Raw �ٿ��� ����ϰ� ������ ����
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate() //���� ���� �����Ӹ��� ȣ��Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager.Instance.isLive)
            return;
        // inputVec.normalized 2��°�� noraml ��
        //�밢���� �����ϰ� �̵� * speed * ���� ������ �ϳ��� �Һ��� �ð�
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // �ٸ� ������ ȯ�濡�� �̵��Ÿ��� �����ϰ� �ϱ� ����
        rigid.MovePosition(rigid.position + nextVec); // ��ġ �̵�
    }

    private void LateUpdate() //�������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager.Instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);//������ ũ���� ��

        if (inputVec.x != 0)
        { //�¿� ���� ��ȯ
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if (GameManager.Instance.health < 0)
        {
            for (int index=2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}