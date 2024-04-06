using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();//�θ� ������Ʈ ��������
    }

    void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);//deltaTime �� �������� �Һ��� �ð�
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //test code
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150; // "-" �ؾ� �ð� �������� ����
                Batch();
                break;
            default:
                speed = 0.3f; // 1�ʿ� 3�߾�
                break;
        }
    }

    void Batch() //count���� poolmanage�� �ִ� �ι�° prefab�� ������
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet; //���� ������Ʈ�� ���� Ȱ��, ���ڶ� ���� Ǯ������ ������
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity; // ȸ�� 000���� �ʱ�ȭ

            Vector3 rotVec = Vector3.forward * 360 * index / count; // ó���� 0, �ι�° 36, ����° 72
            bullet.Rotate(rotVec); // Rotate : �Լ��� ���� ���� ����
            bullet.Translate(bullet.up * 1.5f, Space.World); // �ڽ��� �������� �̵�, 1.5�÷��̾�� �Ÿ�, space world�� �̵�����
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);//-1 is Infinity Per.
        }
    }
    void Fire()
    {
        if (!player.scanner.nearestTarget) //����� ���ٸ�
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; //��ġ
        Vector3 dir = targetPos - transform.position; //ũ�Ⱑ ���Ե� ����: ��ǥ��ġ - ���� ��ġ
        dir = dir.normalized;//���� ������ ������ �����ϰ� ũ�⸦ 1�� ��ȯ�� �Ӽ�

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position; //�÷��̾� ��ġ
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); //������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
