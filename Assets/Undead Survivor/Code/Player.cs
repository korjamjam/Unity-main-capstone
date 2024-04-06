using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 2번째 input 방법

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
    void Update() // 1번째 input 방법
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");//Raw 붙여서 깔끔하게 움직임 가능
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        //대각선도 같이 * speed * 물리 프레임 하나가 소비한 시간
        // inputVec.normalized 2번째는 noraml 뺌
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //3. 위치 이동
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
        anim.SetFloat("Speed",inputVec.magnitude);//순순히 크기의 값

        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
