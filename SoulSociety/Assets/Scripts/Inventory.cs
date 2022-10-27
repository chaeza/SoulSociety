using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    //이 스크립트는 플레이어에게 지급됩니다.
    List<int> inventory = new List<int>();//현재 들어있는 스킬 비교
    private void Start()
    {
        inventory.Add(-1);//리스트 0번배열은 안쓰고 1,2,3,4배열에 -1을넣어 비어있음으로 만듬
        inventory.Add(-1);
        inventory.Add(-1);
        inventory.Add(-1);
        inventory.Add(-1);
    }
    public bool InvetoryCount(int Num)//해당 인벤토리가 비어있는지 판별함
    {
        return inventory[Num] == -1;
    }
    public void AddInventory(int Num)// 해당 배열에 아이템번호를 넣어 가지고 있는 아이템을 판별함
    {
        if (inventory[1] == -1)
        {
            inventory[1] = Num;
            GameMgr.Instance.uiMgr.ItemUI(1, Num);
        }
        else if (inventory[2] == -1)
        {
            inventory[2] = Num;
            GameMgr.Instance.uiMgr.ItemUI(2, Num);
        }
        else if (inventory[3] == -1)
        {
            inventory[3] = Num;
            GameMgr.Instance.uiMgr.ItemUI(3, Num);
        }
        else if (inventory[4] == -1)
        {
            inventory[4] = Num;
            GameMgr.Instance.uiMgr.ItemUI(4, Num);
        }
    }
    public void RemoveInventory(int Num)//해당 배열의 값을 비어있음으로 만듬
    {
        inventory[Num] = -1;
    }
    public bool ContainInventory(int Num)//인벤토리에 해당 아이템번호가 있는지 확인함
    {
        return inventory.Contains(Num);
    }
    public int GetInventory(int Num)
    {
        return inventory[Num];
    }
}
