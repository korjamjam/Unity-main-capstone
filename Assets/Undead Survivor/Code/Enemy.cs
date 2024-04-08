using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))//현재상태 정보 가져오는 함수
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //방향키를 눌러서 생겨난 다음 위치의 양 
        rigid.MovePosition(rigid.position + nextVec);
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
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(health > 0)
        {
            //살아있음, hit action
            anim.SetTrigger("Hit");
        }
        else
        {
            //Die
            Dead();
        }

    }

    IEnumerator KnockBack() //코루틴
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        //플레이어 위치 반대로 
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; //플레이어 반대방향
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); //넉백 3만큼 즉발적
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }    
}
