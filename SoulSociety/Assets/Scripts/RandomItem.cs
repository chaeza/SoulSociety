using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    int itemNum = 4;//�� ������ ����
    int itemRan = 0;//�������� ���� ������ ��ȣ
    public void GetRandomitem(GameObject player)// ���������� ����
    {
        itemRan = Random.Range(0, itemNum);//�����۹�ȣ �̱�
        if (GameMgr.Instance.inventory.InvetoryCount(1) != true && GameMgr.Instance.inventory.InvetoryCount(2) != true && GameMgr.Instance.inventory.InvetoryCount(3) != true && GameMgr.Instance.inventory.InvetoryCount(4) != true)
        {//�κ��丮 1,2,3,4�� ĭ�� ��� á���� ����
            Debug.Log("�κ��丮�� ���� á���ϴ�.");// �������� 4���Ͻ� ���â
            return;
        }
        while (true)
        {
            if (itemRan == 0 && GameMgr.Instance.inventory.ContainInventory(0) == false)//���� ��ȣ�� 1���Ͻ� �κ��丮�� 1�� �������� �ִ��� Ȯ���ϰ� ������ 1�������� ����.
            {
                player.AddComponent<Recovery>();//������ ������Ʈ �߰�
                Cheak(player);
                GameMgr.Instance.inventory.AddInventory(itemRan);//�κ��丮�� ���� ������ ��ȣ�� ����Ʈ�� ����
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
            }//������ �߰��� ���⿡ else if �߰�
            else itemRan = Random.Range(0, itemNum);//�ߺ��� �ٽ� ����
        }
        player.SendMessage("SameItem", itemRan, SendMessageOptions.DontRequireReceiver);//�κ��丮 ������Ʈ�� ������ ������ ������ ����Ʈ�� ����

        //UI �Ŵ�����  ���� �κ��丮������ �Ѱܼ� �ش� ĭ�� �������� ǥ��
    }
    void Cheak(GameObject player)//���� ����ִ� �κ��丮 ĭ���� ���� �����۹�ȣ�� �ű�
    {
        if (GameMgr.Instance.inventory.InvetoryCount(1) == true) player.SendMessage("GetItem", 1, SendMessageOptions.DontRequireReceiver);//������ ������Ʈ�� ���� �κ��丮â�� ���� ����
        else if (GameMgr.Instance.inventory.InvetoryCount(2) == true) player.SendMessage("GetItem", 2, SendMessageOptions.DontRequireReceiver);//������ ������Ʈ�� ���� �κ��丮â�� ���� ����
        else if (GameMgr.Instance.inventory.InvetoryCount(3) == true) player.SendMessage("GetItem", 3, SendMessageOptions.DontRequireReceiver);//������ ������Ʈ�� ���� �κ��丮â�� ���� ����
        else if (GameMgr.Instance.inventory.InvetoryCount(4) == true) player.SendMessage("GetItem", 4, SendMessageOptions.DontRequireReceiver);//������ ������Ʈ�� ���� �κ��丮â�� ���� ����
    }
}
