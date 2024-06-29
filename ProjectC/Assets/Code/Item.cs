using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data; // 아이템 데이터
    public int level; // 아이템 레벨
    public Weapon weapon; // 무기 참조
    public Gear gear; // 기어 참조

    Image icon; // 아이템 아이콘 이미지
    Text textLevel; // 레벨 텍스트
    Text textName; // 이름 텍스트
    Text textDesc; // 설명 텍스트

    void Awake()
    {
        // 아이콘 초기화
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        // 텍스트 초기화
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName; // 아이템 이름 설정
    }

    void OnEnable()
    {
        // 아이템 레벨 텍스트 설정
        textLevel.text = "Lv." + (level + 1);
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // 근접 또는 원거리 무기의 경우 설명 텍스트 설정
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // 글러브 또는 신발의 경우 설명 텍스트 설정
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                // 기타 아이템의 경우 설명 텍스트 설정
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    // 아이템 클릭 시 호출되는 함수
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    // 레벨이 0일 때 새 무기 생성
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    // 무기 레벨 업
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    // 레벨이 0일 때 새 기어 생성
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    // 기어 레벨 업
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                break;

            case ItemData.ItemType.Heal:
                // 회복 아이템일 경우 최대 체력으로 회복
                GameManager.Instance.health = GameManager.Instance.maxHealth;
                break;
        }
        level++; // 아이템 레벨 증가

        // 최대 레벨에 도달하면 버튼 비활성화
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
