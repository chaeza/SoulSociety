using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 5;//�� ��ų ����
    public int skillRan { get; set; } = 0;//�������� ���� ��ų ��ȣ
    public void GetRandomSkill(GameObject player)// ������ų ����
    {
        skillRan = Random.Range(0, skillNum);//��ų��ȣ �̱�
        if (skillRan == 0) player.AddComponent<StoneField>();
        if (skillRan == 1) player.AddComponent<SwordCrash>();
        if (skillRan == 2) player.AddComponent<SwordRain>();
        if (skillRan == 3) player.AddComponent<SwampField>();
        if (skillRan == 4) player.AddComponent<DragonBreath>();
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
    

}
