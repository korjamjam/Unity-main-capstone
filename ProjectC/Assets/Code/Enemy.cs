using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public Rigidbody2D target;
    public RuntimeAnimatorController[] animCon;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    // Update is called once per frame
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }
    void FixedUpdate() // 물리적인 이동을 위한 함수
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Ghit") 
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Mhit") 
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Shit"))//현재상태 정보 가져오는 함수
            return;

        Vector2 dirVec = target.position - rigid.position; // 가는 방향 = 타켓위치 - 나의 위치
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //방향키를 눌러서 생겨난 다음 위치의 양 
        rigid.MovePosition(rigid.position + nextVec); //현재위치 + 다음 위치
        rigid.velocity = Vector2.zero; //물리 속도가 이동에 영향을 주지 않도록속도 제거
    }
    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

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
        anim.SetBool("GDead", false);
        anim.SetBool("MDead", false);
        anim.SetBool("SDead", false);
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
        // 충돌한 객체가 "Bullet" 태그를 가지고 있지 않거나 객체가 살아있지 않으면 함수 종료
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // 객체의 체력을 총알의 데미지 값만큼 감소
        health -= collision.GetComponent<Bullet>().damage;

        // 넉백 효과를 처리하기 위해 KnockBack 코루틴 시작
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // 객체가 아직 살아있으면 맞는 행동 수행
            // "Ghit", "Mhit", "Shit" 트리거를 설정하여 애니메이션 재생
            anim.SetTrigger("Ghit");
            anim.SetTrigger("Mhit");
            anim.SetTrigger("Shit");

            // 히트 사운드 재생
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }

        else
        {
            //Die
            isLive = false;
            coll.enabled = false;//컴포넌트 비활성화
            rigid.simulated = false; //물리를 시뮬레이션 안하는 것
            spriter.sortingOrder = 1; // sprite Renderer에서 order in layer와 같다, 보여지는 단계?
            anim.SetBool("GDead", true); // 죽는 애니메이션, bool인 dead - true 여서
            anim.SetBool("Dead", true);
            anim.SetBool("SDead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();

            if(GameManager.Instance.isLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
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