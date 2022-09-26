using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneField : MonoBehaviour , SkillMethod
{
    bool skillCool = false;
    ResourceData eff;
    public void ResetCooltime()
    {
        skillCool = false;//��ų�� �ٽ� ��� �����ϰ���
        Debug.Log("��ų��");
    }
    public void SkillFire()
    {
        if (skillCool == false)//��ų ��� �����̸�
        {
            GameObject a = Instantiate(eff.stoneField, transform.position, Quaternion.identity);
            a.transform.Translate(0, 1, 2);
            skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
            Debug.Log("��ų���");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 30);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
        }
    }
}
