using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 1;//�� ��ų ����
    public int skillRan { get; set; } = 0;//�������� ���� ��ų ��ȣ
    public void GetRandomSkill(GameObject player)// ������ų ����
    {
        skillRan = Random.Range(0, skillNum);//��ų��ȣ �̱�
        Debug.Log(skillRan);
        if (skillRan == 1) player.AddComponent<StoneField>();
        if (skillRan == 0) player.AddComponent<SpearCrash>();
        if (skillRan == 2) player.AddComponent<SwordRain>();
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
    

}
