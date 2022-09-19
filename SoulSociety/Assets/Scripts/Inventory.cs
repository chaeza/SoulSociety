using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    //이 스크립트는 플레이어에게 지급됩니다.
    public List<int> inventory = new List<int>();
    private void Start()
    {
        inventory.Add(0);
        inventory.Add(0);
        inventory.Add(0);
        inventory.Add(0);
        inventory.Add(0);
        Debug.Log(inventory[1]);
    }
    public bool InvetoryCount(int Num)
    {
        return inventory[Num] == 0;
    }
    public void AddInventory(int Num)
    {
        if (inventory[1] == 0) inventory[1] = Num;
        else if (inventory[2] == 0) inventory[2] = Num;
        else if (inventory[3] == 0) inventory[3] = Num;
        else if (inventory[4] == 0) inventory[4] = Num;
    }
    public void RemoveInventory(int Num)
    {
        inventory[Num] = 0;
    }
    public bool ContainInvetory(int Num)
    {
        return inventory.Contains(Num);
    }
}
