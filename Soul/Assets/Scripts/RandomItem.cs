using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    int itemNum = 2;//�� ������ ����
    int itemRan = 0;//�������� ���� ������ ��ȣ
    public void GetRandomitem(GameObject player)// ���������� ����
    {
        itemRan = Random.Range(1, itemNum + 1);//�����۹�ȣ �̱�
        if (itemRan == 1) 
        if (itemRan == 2) player.AddComponent<Skill2>();
    }

}
