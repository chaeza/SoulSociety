using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 3;//�� ��ų ����
    public int skillRan { get; set; } = 0;//�������� ���� ��ų ��ȣ
    public void GetRandomSkill(GameObject player)// ������ų ����
    {
        skillRan = Random.Range(1, skillNum + 1);//��ų��ȣ �̱�
        if (skillRan == 1) player.AddComponent<StoneField>();
        else if (skillRan == 2) player.AddComponent<SpearCrash>();
        else if (skillRan == 3) player.AddComponent<SwordRain>();
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
    

}
