using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    PlayerMove playerMove;
    RandomSkill randomSkill;
    public RandomSkill myRandomSkill
    {
        get
        {
            if (randomSkill == null)
                randomSkill = FindObjectOfType<RandomSkill>();
            return randomSkill;
        }
    }
    private void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        randomSkill = FindObjectOfType<RandomSkill>();
    }
}
