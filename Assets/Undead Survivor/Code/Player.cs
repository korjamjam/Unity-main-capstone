using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 2��° input ���

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    void Update() // 1��° input ���
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");//Raw �ٿ��� ����ϰ� ������ ����
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        //�밢���� ���� * speed * ���� ������ �ϳ��� �Һ��� �ð�
        // inputVec.normalized 2��°�� noraml ��
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //3. ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);
    }
    /*
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    */
    private void LateUpdate()
    {
        anim.SetFloat("Speed",inputVec.magnitude);//������ ũ���� ��

        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
