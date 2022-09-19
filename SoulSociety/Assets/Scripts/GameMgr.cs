using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    public PlayerInput playerInput;
    public Inventory inventory;
    public RandomSkill randomSkill;
    public RandomItem randomItem;
    public GameObject test; // ���߿� ���� , �÷��̾�� ��Ʈ�� ������ ������ ����
    
  
    private void Awake()
    {
        //Instantiate(test, Vector3.zero, Quaternion.identity);
        
        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        playerInput = gameObject.AddComponent<PlayerInput>();
        inventory = gameObject.AddComponent<Inventory>();
    }
}
