using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Scriptable Object/ItemData")]
public class ItemData : ScriptableObject    //scriptable object �ʼ�
{
    public enum ItemType {Melee, Range, Glove, Shoe, Heal, Blade }


    [Header("# Main Info")]
    public ItemType Type; 
    public int itemId;      //Id
    public string itemName; //�̸�
    [TextArea]   //�� ���ڸ� 2�� �̻� �ۼ� ����
    public string itemDesc; //����
    public Sprite itemIcon; //������

    [Header("# Level Data")]
    public float baseDamage;    //�⺻������
    public int baseCount;   //�⺻����
    public float[] damages; //������ ����������
    public int[] counts;  //������ ��������

    [Header("# Weapon")]
    public GameObject projecTile; //����ü ������Ʈ
}
