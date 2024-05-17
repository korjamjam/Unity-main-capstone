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
    void FixedUpdate() // 물리적인 이동을 위한 함수
    {
        if (!isLive)//현재상태 정보 가져오는 함수
            return;

        Vector2 dirVec = target.position - rigid.position; // 가는 방향 = 타켓위치 - 나의 위치
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //방향키를 눌러서 생겨난 다음 위치의 양 
        rigid.MovePosition(rigid.position + nextVec); //현재위치 + 다음 위치
        rigid.velocity = Vector2.zero; //물리 속도가 이동에 영향을 주지 않도록속도 제거
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