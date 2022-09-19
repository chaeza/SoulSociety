using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    public PlayerInput playerInput;
    public RandomSkill randomSkill;
    public GameObject test; // ���߿� ���� , �÷��̾�� ��Ʈ�� ������ ������ ����
    
  
    private void Awake()
    {
        Instantiate(test, Vector3.zero, Quaternion.identity);
        
        randomSkill = gameObject.AddComponent<RandomSkill>();
        playerInput = gameObject.AddComponent<PlayerInput>();
    }
}
