using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1 : MonoBehaviour, ItemMethod//������ �������̽� ���
{
    [SerializeField]
    int itemNum = 0;
    public void GetItem(int itemnum)//�ش� �������� ��� �κ��丮�� �ִ��� ���� å��
    {
        if (itemNum == 0)
            itemNum = itemnum;
    }

    public void ItemFire()//�÷��̾� Attack���� Ű�Է½� �ش� �������� �κ��丮 ��ġ�� ���Ͽ� ������ ��ų ��� 
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }
    public void ItemSkill()//�� �������� ������ �ִ� ������ ��ų ���
    {
        // ��ų ����





        //
        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);//�κ��丮���� �� ��ų�� ������ ���� �ʱ�ȭ��
        Destroy(GetComponent<Item1>());//�ش� �������� ����� ������Ʈ ����
    }
}
