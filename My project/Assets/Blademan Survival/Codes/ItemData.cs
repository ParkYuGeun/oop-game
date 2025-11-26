using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Scriptable Object/ItemData")]
public class ItemData : ScriptableObject    //scriptable object 필수
{
    public enum ItemType {Melee, Range, Glove, Shoe, Heal, Blade }


    [Header("# Main Info")]
    public ItemType Type; 
    public int itemId;      //Id
    public string itemName; //이름
    [TextArea]   //밑 문자를 2줄 이상 작성 가능
    public string itemDesc; //설명
    public Sprite itemIcon; //아이콘

    [Header("# Level Data")]
    public float baseDamage;    //기본데미지
    public int baseCount;   //기본관통
    public float[] damages; //레벨별 데미지증가
    public int[] counts;  //레벨별 관통증가

    [Header("# Weapon")]
    public GameObject projecTile; //투사체 오브젝트
}
