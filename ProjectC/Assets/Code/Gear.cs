using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    // 기어 타입과 속도 비율을 나타내는 변수
    public ItemData.ItemType type;
    public float rate;

    // 기어 초기화 함수
    public void Init(ItemData data)
    {
        // 기본 설정
        name = "Gear" + data.itemId;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        // 속성 설정
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    // 기어 레벨 업 함수
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    // 기어 적용 함수
    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    // 무기의 속도 증가 함수
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    // 무기 속도를 비율에 따라 증가
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:
                    // 기타 무기 속도를 비율에 따라 감소
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }

    // 플레이어의 속도 증가 함수
    void SpeedUp()
    {
        float speed = 3;
        GameManager.Instance.player.speed = speed + speed * rate;
    }
}
