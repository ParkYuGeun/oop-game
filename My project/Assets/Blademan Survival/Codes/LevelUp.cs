using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] Items;

     void Awake()
    {
         rect = GetComponent<RectTransform>();
        Items = GetComponentsInChildren<Item>(true);    //활성화 되어있는 오브젝트만 가져오기
    }

    public void Show()  //게임매니저에서 컨트롤
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.Instance.stop();

    }

    public void Hide()  //하위오브젝튿의 버튼에서 컨트롤
    {
        rect.localScale = Vector3.zero;
        GameManager.Instance.resume();

    }

    public void select(int index) { //게임매니저에서 처음 1번 사용
        Items[index].onClick();
       
    }

    void Next() {
        //전부 비활성화
        foreach (Item item in Items) {  
            item.gameObject.SetActive(false);   
        }
        //무작위아이템 중 3개 선정, 중복 허용x 
        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0,Items.Length);
            ran[1] = Random.Range(0, Items.Length);
            ran[2] = Random.Range(0, Items.Length);

            if (ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2]) {
                break;          
            }
        }
        for (int index = 0; index < ran.Length; index++) {
            Item ranItem = Items[ran[index]];
            //선정된 아이템이 만렙이면 치료아이템으로 선정
            if (ranItem.level == ranItem.data.damages.Length)
            {
                Items[Random.Range(4,4)].gameObject.SetActive(true);
            }
            else {
                ranItem.gameObject.SetActive(true);
            }
        }
    
    }
}
