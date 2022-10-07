using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 9;//총 스킬 갯수
    public int skillRan { get; set; } = 0;//랜덤으로 뽑을 스킬 번호
    public void GetRandomSkill(GameObject player)// 랜덤스킬 지급
    {
        skillRan = Random.Range(0, skillNum);//스킬번호 뽑기
       // skillRan = 8;//테스트시 원하는 스킬 고르기
        if (skillRan == 0) player.AddComponent<StoneField>();
        if (skillRan == 1) player.AddComponent<SwordCrash>();
        if (skillRan == 2) player.AddComponent<SwordRain>();
        if (skillRan == 3) player.AddComponent<SwampField>();
        if (skillRan == 4) player.AddComponent<WindCyclone>();
        if (skillRan == 5) player.AddComponent<DevilSword>();
        if (skillRan == 6) player.AddComponent<DevilEye>();
        if (skillRan == 7) player.AddComponent<DashAttack>();
        if (skillRan == 8) player.AddComponent<ControlMissie>();
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
    

}
