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
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
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
        isLive = true; // 5줄 재활용 위해 ontrigger에서 가져옴
        coll.enabled = true; 
        rigid.simulated = true; 
        spriter.sortingOrder = 2; // sprite Renderer에서 order in layer와 같다, 보여지는 단계?
        anim.SetBool("Dead", false); 
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
        if (!collision.CompareTag("Bullet") || !isLive)
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
            isLive = false;
            coll.enabled = false;//컴포넌트 비활성화
            rigid.simulated = false; //물리를 시뮬레이션 안하는 것
            spriter.sortingOrder = 1; // sprite Renderer에서 order in layer와 같다, 보여지는 단계?
            anim.SetBool("Dead",true); // 죽는 애니메이션, bool인 dead - true 여서
            GameManager.Instance.kill++;
            GameManager.Instance.Getexp();
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
