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
    void FixedUpdate() // �������� �̵��� ���� �Լ�
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Ghit") 
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Mhit") 
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Shit"))//������� ���� �������� �Լ�
            return;

        Vector2 dirVec = target.position - rigid.position; // ���� ���� = Ÿ����ġ - ���� ��ġ
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //����Ű�� ������ ���ܳ� ���� ��ġ�� �� 
        rigid.MovePosition(rigid.position + nextVec); //������ġ + ���� ��ġ
        rigid.velocity = Vector2.zero; //���� �ӵ��� �̵��� ������ ���� �ʵ��ϼӵ� ����
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
        isLive = true; // 5�� ��Ȱ�� ���� ontrigger���� ������
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2; // sprite Renderer���� order in layer�� ����, �������� �ܰ�?
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
        // �浹�� ��ü�� "Bullet" �±׸� ������ ���� �ʰų� ��ü�� ������� ������ �Լ� ����
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // ��ü�� ü���� �Ѿ��� ������ ����ŭ ����
        health -= collision.GetComponent<Bullet>().damage;

        // �˹� ȿ���� ó���ϱ� ���� KnockBack �ڷ�ƾ ����
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // ��ü�� ���� ��������� �´� �ൿ ����
            // "Ghit", "Mhit", "Shit" Ʈ���Ÿ� �����Ͽ� �ִϸ��̼� ���
            anim.SetTrigger("Ghit");
            anim.SetTrigger("Mhit");
            anim.SetTrigger("Shit");

            // ��Ʈ ���� ���
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }

        else
        {
            //Die
            isLive = false;
            coll.enabled = false;//������Ʈ ��Ȱ��ȭ
            rigid.simulated = false; //������ �ùķ��̼� ���ϴ� ��
            spriter.sortingOrder = 1; // sprite Renderer���� order in layer�� ����, �������� �ܰ�?
            anim.SetBool("GDead", true); // �״� �ִϸ��̼�, bool�� dead - true ����
            anim.SetBool("Dead", true);
            anim.SetBool("SDead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();

            if(GameManager.Instance.isLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }
    IEnumerator KnockBack() //�ڷ�ƾ
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        //�÷��̾� ��ġ �ݴ�� 
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; //�÷��̾� �ݴ����
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); //�˹� 3��ŭ �����
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}