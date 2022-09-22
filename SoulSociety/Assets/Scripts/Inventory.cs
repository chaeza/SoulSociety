using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    //�� ��ũ��Ʈ�� �÷��̾�� ���޵˴ϴ�.
    List<int> inventory = new List<int>();//���� ����ִ� ��ų ��
    private void Start()
    {
        inventory.Add(0);//����Ʈ 0���迭�� �Ⱦ��� 1,2,3,4�迭�� 0���־� ����������� ����
        inventory.Add(0);
        inventory.Add(0);
        inventory.Add(0);
        inventory.Add(0);
        Debug.Log(inventory[1]);
    }
    public bool InvetoryCount(int Num)//�ش� �κ��丮�� ����ִ��� �Ǻ���
    {
        return inventory[Num] == 0;
    }
    public void AddInventory(int Num)// �ش� �迭�� �����۹�ȣ�� �־� ������ �ִ� �������� �Ǻ���
    {
        if (inventory[1] == 0)
        {
            inventory[1] = Num;
            GameMgr.Instance.uIMgr.ItemUI(1, Num);
        }
        else if (inventory[2] == 0)
        {
            inventory[2] = Num;
            GameMgr.Instance.uIMgr.ItemUI(2, Num);
        }
        else if (inventory[3] == 0)
        {
            inventory[3] = Num;
            GameMgr.Instance.uIMgr.ItemUI(3, Num);
        }
        else if (inventory[4] == 0)
        {
            inventory[4] = Num;
            GameMgr.Instance.uIMgr.ItemUI(4, Num);
        }
    }
    public void RemoveInventory(int Num)//�ش� �迭�� ���� ����������� ����
    {
        inventory[Num] = 0;
    }
    public bool ContainInventory(int Num)//�κ��丮�� �ش� �����۹�ȣ�� �ִ��� Ȯ����
    {
        return inventory.Contains(Num);
    }
    public int GetInventory(int Num)
    {
        return inventory[Num];
    }
}
