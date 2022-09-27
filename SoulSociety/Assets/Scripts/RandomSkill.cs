using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 2;//�� ��ų ����
    public int skillRan { get; set; } = 0;//�������� ���� ��ų ��ȣ
    public void GetRandomSkill(GameObject player)// ������ų ����
    {
        skillRan = Random.Range(0, skillNum);//��ų��ȣ �̱�
        if (skillRan == 0) player.AddComponent<Skill>();
        if (skillRan == 1) player.AddComponent<Skill2>();
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
    

}
