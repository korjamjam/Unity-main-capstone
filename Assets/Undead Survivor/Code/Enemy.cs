using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed; //�ӵ� ����
    public float health; 
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target; //��ǥ ����

    bool isLive; //�������� ����

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

    void FixedUpdate() // �������� �̵��� ���� �Լ�
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))//������� ���� �������� �Լ�
            return;

        Vector2 dirVec = target.position - rigid.position; // ���� ���� = Ÿ����ġ - ���� ��ġ
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //����Ű�� ������ ���ܳ� ���� ��ġ�� �� 
        rigid.MovePosition(rigid.position + nextVec); //������ġ + ���� ��ġ
        rigid.velocity = Vector2.zero; //���� �ӵ��� �̵��� ������ ���� �ʵ��ϼӵ� ����
    }

    void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true; // 5�� ��Ȱ�� ���� ontrigger���� ������
        coll.enabled = true; 
        rigid.simulated = true; 
        spriter.sortingOrder = 2; // sprite Renderer���� order in layer�� ����, �������� �ܰ�?
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
            //�������, hit action
            anim.SetTrigger("Hit");
        }
        else
        {
            //Die
            isLive = false;
            coll.enabled = false;//������Ʈ ��Ȱ��ȭ
            rigid.simulated = false; //������ �ùķ��̼� ���ϴ� ��
            spriter.sortingOrder = 1; // sprite Renderer���� order in layer�� ����, �������� �ܰ�?
            anim.SetBool("Dead",true); // �״� �ִϸ��̼�, bool�� dead - true ����
            GameManager.Instance.kill++;
            GameManager.Instance.Getexp();
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
