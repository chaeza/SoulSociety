using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour, SkillMethod
{
    bool skillCool = false;
    public void ResetCooltime()
    {
        skillCool = false;//��ų�� �ٽ� ��� �����ϰ���
        Debug.Log("��ų��");
    }
    public void SkillFire()
    {
        if (skillCool == false)//��ų ��� �����̸�
        {
            skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
            Debug.Log("��ų���");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 5);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
        }
    }
}

