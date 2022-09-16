using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 2;//총 스킬 갯수
    int skillRan = 0;//랜덤으로 뽑을 스킬 번호
    public void GetRandomSkill(GameObject player)// 랜덤스킬 지급
    {
        skillRan= Random.Range(1, skillNum + 1);//스킬번호 뽑기
        if (skillRan == 1) player.AddComponent<Skill>();
        if (skillRan == 2) player.AddComponent<Skill2>();
    }

}
