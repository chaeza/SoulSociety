using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    public PlayerInput playerInput;
    public RandomSkill randomSkill;
    public GameObject test; // 나중에 삭제 , 플레이어는 스트링 값으로 프리팹 생성
    
  
    private void Awake()
    {
        Instantiate(test, Vector3.zero, Quaternion.identity);
        
        randomSkill = gameObject.AddComponent<RandomSkill>();
        playerInput = gameObject.AddComponent<PlayerInput>();
    }
}
