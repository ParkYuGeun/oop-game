using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;  //데이터에서 무기가 아닌것들
    public float rate;  //레벨별 수치

    public void Init(ItemData data) {
        //Basic
        name = "Gear"+data.itemId;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;
        //Property
        type = data.Type;   //구분용
        rate = data.damages[0]; //수치
        ApplyGear();
    }

    public void LevelUp(float rate) {
        this.rate = rate;
        ApplyGear();
    }

    public void ApplyGear()
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

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id) {
                case 0:     //근거리일때
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:    //원거리일때
                    weapon.speed = 0.5f*(1f-rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3;
        GameManager.Instance.player.speed = speed+speed*rate;
    }
}
