using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    public PlayerInput playerInput;
    public FollowCam followCam;
    public Inventory inventory;
    public RandomSkill randomSkill;
    public RandomItem randomItem;
    public UIMgr uIMgr;
    public GameObject test; // ���߿� ���� , �÷��̾�� ��Ʈ�� ������ ������ ����
    [SerializeField] GameObject palyer = null;   
    
  
    private void Awake()
    {
        //Instantiate(test, Vector3.zero, Quaternion.identity);
      
        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        playerInput = gameObject.AddComponent<PlayerInput>();
        inventory = gameObject.AddComponent<Inventory>();
        uIMgr = FindObjectOfType<UIMgr>();
    }

    private void Start()
    {
        Instantiate(palyer, new Vector3(-2, 49, -8), Quaternion.identity);
      
        GameObject playerr = GameObject.Find("Character_Chost(Clone)");
        followCam = FindObjectOfType<FollowCam>();
        followCam.playerStart(playerr.transform);
    }

}
