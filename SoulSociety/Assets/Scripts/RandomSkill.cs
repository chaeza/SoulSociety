using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    int skillNum = 1;
    int skillRan = 0;
    public void GetRandomSkill(GameObject player)
    {
        skillRan= Random.Range(1, skillNum + 1);
        Debug.Log(skillRan);
        if (skillRan == 1) player.AddComponent<Skill>();
    }
}
