using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 2번째 input 방법

public class Player1 : MonoBehaviour // 게임 로직 구성에 필요한 것들을 가진 클래스
{
    public Vector2 inputVec;
    public float speed;
    /*public Scanner scanner;*/
    Rigidbody2D rigid;//물리 엔진
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //오브젝트에서 컨포넌트를 가져오는 함수
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        /*scanner = GetComponent<Scanner>();*/
    }
    void Update() // 1번째 input 방법
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");//Raw 붙여서 깔끔하게 움직임 가능
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate() //물리 연산 프레임마다 호출되는 생명주기 함수
    {
        // inputVec.normalized 2번째는 noraml 뺌
        //대각선도 동일하게 이동 * speed * 물리 프레임 하나가 소비한 시간
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 다른 프레임 환경에도 이동거리는 동일하게 하기 위함
        rigid.MovePosition(rigid.position + nextVec); // 위치 이동
    }
    /*
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    */
    private void LateUpdate() //프레임이 종료되기 전 실행되는 생명주기 함수
    {
        anim.SetFloat("Speed", inputVec.magnitude);//순순히 크기의 값

        if (inputVec.x != 0)
        { //좌우 방향 전환
            spriter.flipX = inputVec.x < 0;
        }
    }
}