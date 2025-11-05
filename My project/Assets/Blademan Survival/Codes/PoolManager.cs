using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리팹보관
    public GameObject[] prefabs;
    //풀 담당 리스트(프리팹과 1:1)
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];   //배열 초기화
        for (int i = 0; i < pools.Length; i++){
            pools[i] = new List<GameObject> ();
        }
    }


    public GameObject Get(int index) //index= 풀번호
    {
        GameObject select = null;
        //선택한 풀의 비활성화된 게임오브젝트 접근
        //발견 시 select에 할당
        
        foreach (GameObject item in pools[index]) {  //pools가 gameobject라서 
            if (!item.activeSelf) {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //못찾으면 생성후 select에 할당
        if (!select) {
        select = Instantiate(prefabs[index],transform); //생성, transform은 poolmanager에 생성한다는 뜻
        pools[index].Add(select);
            }
        return select;
    }

}
