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
                transform.Rotate(Vector3.back * speed * Time.deltaTime);//deltaTime 한 프레임이 소비한 시간
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
                speed = 150; // "-" 해야 시계 방향으로 돌음
                Batch();
                break;
            default:
                speed = 0.3f; // 1초에 3발씩
                break;
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet; //기존 오브젝트를 먼저 활용, 모자란 것은 풀링에서 가져옴
            if (index < transform.childCount) // 기존 근접무기 재활용
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity; // 회전 000으로 초기화

            Vector3 rotVec = Vector3.forward * 360 * index / count; // 처음은 0, 두번째 36, 세번째 72
            bullet.Rotate(rotVec); // Rotate : 함수로 계산된 각도 적용
            bullet.Translate(bullet.up * 1.5f, Space.World); // 자신의 위쪽으로 이동, 1.5플레이어와 거리, space world는 이동방향
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);//-1 is Infinity Per.

            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Melee);
        }
    }
    void Fire()
    {
        if (!player.scanner.nearestTarget) //대상이 없다면
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; //위치
        Vector3 dir = targetPos - transform.position; //크기가 포함된 방향: 목표위치 - 나의 위치
        dir = dir.normalized;//현재 벡터의 방향은 유지하고 크기를 1로 변환된 속성

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position; //플레이어 위치
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); //지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }

}
