using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneField : MonoBehaviour , SkillMethod
{
    bool skillCool = false;
    ResourceData eff;
    public void ResetCooltime()
    {
        skillCool = false;//스킬을 다시 사용 가능하게함
        Debug.Log("스킬쿨끝");
    }
    public void SkillFire()
    {
        if (skillCool == false)//스킬 사용 가능이면
        {
            GameObject a = Instantiate(eff.stoneField, transform.position, Quaternion.identity);
            a.transform.Translate(0, 1, 2);
            skillCool = true;//쿨타임 온 시켜 다시 사용 못하게함
            Debug.Log("스킬사용");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 30);//UI매니저에 쿨타임 10초를 보냄
        }
    }
}
