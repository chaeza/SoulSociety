using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 2;//�� ��ų ����
    int skillRan = 0;//�������� ���� ��ų ��ȣ
    public void GetRandomSkill(GameObject player)// ������ų ����
    {
        skillRan= Random.Range(1, skillNum + 1);//��ų��ȣ �̱�
        if (skillRan == 1) player.AddComponent<Skill>();
        if (skillRan == 2) player.AddComponent<Skill2>();
    }

}
