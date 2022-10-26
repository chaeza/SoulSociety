using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 10;//�� ��ų ����
    public int skillRan { get; set; } = 0;//�������� ���� ��ų ��ȣ
    public void GetRandomSkill(GameObject player)// ������ų ����
    {
        skillRan = Random.Range(0, skillNum);//��ų��ȣ �̱�
        skillRan = 9;//�׽�Ʈ�� ���ϴ� ��ų ����
        if (skillRan == 0) player.AddComponent<StoneField>();
        else if (skillRan == 1) player.AddComponent<SwordCrash>();
        else if (skillRan == 2) player.AddComponent<SwordRain>();
        else if (skillRan == 3) player.AddComponent<SwampField>();
        else if (skillRan == 4) player.AddComponent<WindCyclone>();
        else if (skillRan == 5) player.AddComponent<DevilSword>();
        else if (skillRan == 6) player.AddComponent<DevilEye>();
        else if (skillRan == 7) player.AddComponent<DashAttack>();
        else if (skillRan == 8) player.AddComponent<ControlMissie>();
        else if (skillRan == 9) player.AddComponent<BloodAttack>();
        //  else if (skillRan == 9) player.0AddComponent<Skill_BasicMissile>();

        else
            Debug.Log("Player didn't get any skil");
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }


}
