using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    int itemNum = 4;//총 아이템 갯수
    int itemRan = 0;//랜덤으로 뽑을 아이템 번호
    public void GetRandomitem(GameObject player)// 랜덤아이템 지급
    {
        itemRan = Random.Range(1, itemNum + 1);//아이템번호 뽑기
        if (GameMgr.Instance.inventory.InvetoryCount(1) != true && GameMgr.Instance.inventory.InvetoryCount(2) != true && GameMgr.Instance.inventory.InvetoryCount(3) != true && GameMgr.Instance.inventory.InvetoryCount(4) != true)
        {
            Debug.Log("인벤토리가 가득 찼습니다.");// 아이템이 4개일시 경고창
            return;
        }
        while (true)
        {
            if (itemRan == 1 && GameMgr.Instance.inventory.ContainInvetory(1) == false)
            {
                player.AddComponent<Item1>();
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);
                break;
            }
            else if (itemRan == 2 && GameMgr.Instance.inventory.ContainInvetory(2) == false)
            {
                player.AddComponent<Item2>();
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);
                break;
            }
            else if (itemRan == 3 && GameMgr.Instance.inventory.ContainInvetory(3) == false)
            {
                player.AddComponent<Item3>();
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);
                break;
            }
            else if (itemRan == 4 && GameMgr.Instance.inventory.ContainInvetory(4) == false)
            {
                player.AddComponent<Item4>();
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);
                break;
            }
            else itemRan = Random.Range(1, itemNum + 1);//중복시 다시 랜덤
        }
        player.SendMessage("SameItem", itemRan, SendMessageOptions.DontRequireReceiver);//인벤토리 컴포넌트에 소지한 아이템 종류를 리스트에 더함

        //UI 매니저에  현재 인벤토리순서를 넘겨서 해당 칸에 아이콘을 표시
    }
    void Cheak(GameObject player)
    {
        if (GameMgr.Instance.inventory.InvetoryCount(1) == true) player.SendMessage("GetItem", 1, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
        else if (GameMgr.Instance.inventory.InvetoryCount(2) == true) player.SendMessage("GetItem", 2, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
        else if (GameMgr.Instance.inventory.InvetoryCount(3) == true) player.SendMessage("GetItem", 3, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
        else if (GameMgr.Instance.inventory.InvetoryCount(4) == true) player.SendMessage("GetItem", 4, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
    }

}
