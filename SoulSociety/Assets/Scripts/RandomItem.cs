using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    int itemNum = 4;//총 아이템 갯수
    int itemRan = 0;//랜덤으로 뽑을 아이템 번호
    public void GetRandomitem(GameObject player)// 랜덤아이템 지급
    {
        itemRan = Random.Range(0, itemNum);//아이템번호 뽑기
        if (GameMgr.Instance.inventory.InvetoryCount(1) != true && GameMgr.Instance.inventory.InvetoryCount(2) != true && GameMgr.Instance.inventory.InvetoryCount(3) != true && GameMgr.Instance.inventory.InvetoryCount(4) != true)
        {//인벤토리 1,2,3,4의 칸이 모두 찼을때 리턴
            Debug.Log("인벤토리가 가득 찼습니다.");// 아이템이 4개일시 경고창
            return;
        }
        while (true)
        {
            if (itemRan == 0 && GameMgr.Instance.inventory.ContainInventory(0) == false)//뽑은 번호가 1번일시 인벤토리에 1번 아이템이 있는지 확인하고 없으면 1번아이템 지급.
            {
                player.AddComponent<Recovery>();//아이템 컴포넌트 추가
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);//인벤토리에 현재 아이템 번호를 리스트에 저장
                break;
            }
            else if (itemRan == 1 && GameMgr.Instance.inventory.ContainInventory(1) == false)
            {
                player.AddComponent<BasicAttackDamageUP>();
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);
                break;
            }
            else if (itemRan == 2 && GameMgr.Instance.inventory.ContainInventory(2) == false)
            {
                player.AddComponent<Trap>();
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);
                break;
            }
            else if (itemRan == 3 && GameMgr.Instance.inventory.ContainInventory(3) == false)
            {
                player.AddComponent<Slash>();
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);
                break;
            }//아이템 추가시 여기에 else if 추가
            else itemRan = Random.Range(0, itemNum);//중복시 다시 랜덤
        }
        player.SendMessage("SameItem", itemRan, SendMessageOptions.DontRequireReceiver);//인벤토리 컴포넌트에 소지한 아이템 종류를 리스트에 더함

        //UI 매니저에  현재 인벤토리순서를 넘겨서 해당 칸에 아이콘을 표시
    }
    void Cheak(GameObject player)//현재 비어있는 인벤토리 칸수에 현재 아이템번호를 매김
    {
        if (GameMgr.Instance.inventory.InvetoryCount(1) == true) player.SendMessage("GetItem", 1, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
        else if (GameMgr.Instance.inventory.InvetoryCount(2) == true) player.SendMessage("GetItem", 2, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
        else if (GameMgr.Instance.inventory.InvetoryCount(3) == true) player.SendMessage("GetItem", 3, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
        else if (GameMgr.Instance.inventory.InvetoryCount(4) == true) player.SendMessage("GetItem", 4, SendMessageOptions.DontRequireReceiver);//아이템 컴포넌트에 남은 인벤토리창에 순서 저장
    }
}
