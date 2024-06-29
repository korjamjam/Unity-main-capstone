using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player1 player;

    void Awake()
    {
        player = GameManager.Instance.player;
    }

    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);//deltaTime �� �������� �Һ��� �ð�
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
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

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for(int index=0; index<GameManager.Instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

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

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet; //���� ������Ʈ�� ���� Ȱ��, ���ڶ� ���� Ǯ������ ������
            if (index < transform.childCount) // ���� �������� ��Ȱ��
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

            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Melee);
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

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }

}
