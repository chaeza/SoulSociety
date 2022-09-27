using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 5;//ÃÑ ½ºÅ³ °¹¼ö
    public int skillRan { get; set; } = 0;//·£´ýÀ¸·Î »ÌÀ» ½ºÅ³ ¹øÈ£
    public void GetRandomSkill(GameObject player)// ·£´ý½ºÅ³ Áö±Þ
    {
        skillRan = Random.Range(1, skillNum + 1);//½ºÅ³¹øÈ£ »Ì±â
        if (skillRan == 1) player.AddComponent<Skill>();
        else if (skillRan == 2) player.AddComponent<Skill2>();
        else if (skillRan == 3) player.AddComponent<StoneField>();
        else if (skillRan == 4) player.AddComponent<SpearCrash>();
        else if (skillRan == 5) player.AddComponent<SwordRain>();
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
    

}
