using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image Icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        Icon = GetComponentsInChildren<Image>()[1];
        Icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }
    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Katana:
            case ItemData.ItemType.Bullet0:
            case ItemData.ItemType.Bullet1:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
            break;
            // 아이템이 추가된다면 여기에 case를 추가
            // 여기는 매개변수 1개인 아이템의 경우 추가, 위는 2개인 경우
            default: // 설명 외에 구태여 매개변수가 필요 없는 desc라면 여기에 추가 
                break;
        }   
    }

    public void OnClick()
    {
        switch (data.itemType)
        {   // 아이템 필요시 추가 요망
            case ItemData.ItemType.Katana:
            case ItemData.ItemType.Bullet0:
            case ItemData.ItemType.Bullet1:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {   
                    // 수식 필요시 변경 요망
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.Levelup(nextDamage, nextCount);
                }
                break;
        }

        level++;

        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
