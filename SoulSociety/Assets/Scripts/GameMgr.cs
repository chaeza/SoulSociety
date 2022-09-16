using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    public PlayerMove playerMove;
    public PlayerAttack playerAttack;
    public RandomSkill randomSkill;


    public GameObject test; // ���߿� ���� , �÷��̾�� ��Ʈ�� ������ ������ ����
    
  
    private void Awake()
    {
        Instantiate(test, Vector3.zero, Quaternion.identity);
        Instantiate(test, Vector3.zero, Quaternion.identity);
        Instantiate(test, Vector3.zero, Quaternion.identity);
        
        playerMove = FindObjectOfType<PlayerMove>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        randomSkill = gameObject.AddComponent<RandomSkill>();
    }
}
