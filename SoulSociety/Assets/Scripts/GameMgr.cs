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
    public HpBarInfo hpBarInfo;

    public bool endGame { get; set; } = false;
    public int dieCount = 0;
    public int redCount = 0;
    public int blueCount = 0;
    private void Awake()
    {
        //Instantiate(test, Vector3.zero, Quaternion.identity);

        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        playerInput = gameObject.AddComponent<PlayerInput>();
        inventory = gameObject.AddComponent<Inventory>();
        //hpBarInfo = FindObjectOfType<HpBarInfo>();
        uIMgr = FindObjectOfType<UIMgr>();
        followCam = FindObjectOfType<FollowCam>();
    }

    public void GetRedSoul(int redsoul)
    {
        blueCount=0;
        if (redsoul == 0) redCount++;
        else redCount += redsoul+1;
        if (redCount >= 3)
        {
            uIMgr.photonView.RPC("EndGame", RpcTarget.All, 1, redCount);
        }
        uIMgr.MyRedSoul(redCount);
    }
    public void GetBuleSoul()
    {
        blueCount++;
        uIMgr.MyBlueSoul(blueCount);
        if(blueCount>=5)
        {
            uIMgr.photonView.RPC("EndGame", RpcTarget.All,2,blueCount);
        }
    }


}
