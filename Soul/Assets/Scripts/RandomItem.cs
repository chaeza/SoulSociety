using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    int itemNum = 2;//총 아이템 갯수
    int itemRan = 0;//랜덤으로 뽑을 아이템 번호
    public void GetRandomitem(GameObject player)// 랜덤아이템 지급
    {
        itemRan = Random.Range(1, itemNum + 1);//아이템번호 뽑기
        if (itemRan == 1) 
        if (itemRan == 2) player.AddComponent<Skill2>();
    }

}
