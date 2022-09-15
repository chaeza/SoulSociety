using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    public PlayerMove playerMove;
    public RandomSkill randomSkill;
    public GameObject test;
    //public RandomSkill myRandomSkill
    //{
    //    get
    //    {
    //        if (randomSkill == null)
    //            randomSkill = FindObjectOfType<RandomSkill>();
    //        return randomSkill;
    //    }
    //}
    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        randomSkill = FindObjectOfType<RandomSkill>();
        Instantiate(test, Vector3.zero, Quaternion.identity);
        Instantiate(test, Vector3.zero, Quaternion.identity);
        Instantiate(test, Vector3.zero, Quaternion.identity);
    }
}
