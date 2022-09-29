using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 1;//ÃÑ ½ºÅ³ °¹¼ö
    public int skillRan { get; set; } = 0;//·£´ýÀ¸·Î »ÌÀ» ½ºÅ³ ¹øÈ£
    public void GetRandomSkill(GameObject player)// ·£´ý½ºÅ³ Áö±Þ
    {
        skillRan = Random.Range(0, skillNum);//½ºÅ³¹øÈ£ »Ì±â
        Debug.Log(skillRan);
        if (skillRan == 1) player.AddComponent<StoneField>();
        if (skillRan == 0) player.AddComponent<SpearCrash>();
        if (skillRan == 2) player.AddComponent<SwordRain>();
        GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
    

}
