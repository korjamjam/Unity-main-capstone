using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    // ��� Ÿ�԰� �ӵ� ������ ��Ÿ���� ����
    public ItemData.ItemType type;
    public float rate;

    // ��� �ʱ�ȭ �Լ�
    public void Init(ItemData data)
    {
        // �⺻ ����
        name = "Gear" + data.itemId;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        // �Ӽ� ����
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    // ��� ���� �� �Լ�
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    // ��� ���� �Լ�
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

    // ������ �ӵ� ���� �Լ�
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    // ���� �ӵ��� ������ ���� ����
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:
                    // ��Ÿ ���� �ӵ��� ������ ���� ����
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }

    // �÷��̾��� �ӵ� ���� �Լ�
    void SpeedUp()
    {
        float speed = 3;
        GameManager.Instance.player.speed = speed + speed * rate;
    }
}
