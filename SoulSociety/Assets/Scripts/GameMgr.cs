using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameMgr : Singleton<GameMgr>

{
   // public FollowCam followCam;
    public PlayerInput playerInput;
    public FollowCam followCam;
    public Inventory inventory;
    public RandomSkill randomSkill;
    public RandomItem randomItem;
    public UIMgr uIMgr;
    public GameObject test; // ���߿� ���� , �÷��̾�� ��Ʈ�� ������ ������ ����
    
  
    private void Awake()
    {
        //Instantiate(test, Vector3.zero, Quaternion.identity);
      
        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        playerInput = gameObject.AddComponent<PlayerInput>();
        inventory = gameObject.AddComponent<Inventory>();
        uIMgr = FindObjectOfType<UIMgr>();
        followCam = FindObjectOfType<FollowCam>();
    }
}
