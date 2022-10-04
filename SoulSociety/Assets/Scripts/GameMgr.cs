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
    public SpawnMgr spawnMgr;
    public ResourceData resourceData;
    public bool endGame { get; set; } = false;
    public int dieCount = 0;
    public int redCount = 0;
    public int blueCount = 0;
    int playerNum = 3;

    int alivePlayerNum = 0;

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
        spawnMgr = FindObjectOfType<SpawnMgr>();
        resourceData = Resources.Load<ResourceData>("ResourceData");
    }
    public void GetRedSoul(int redsoul)
    {
        int num = 0;
        PlayerInfo[] playerinfo = FindObjectsOfType<PlayerInfo>();
        for (int i = 0; i < playerinfo.Length; i++)
        {
            if (playerinfo[i].playerState == state.Die) num++;
        }


        if (redsoul == 0) redCount++;

        else redCount += redsoul + 1;

        /*       if (PhotonNetwork.PlayerList.Length - num == 1)
               {
                   uIMgr.photonView.RPC("EndGame", RpcTarget.All, 1, redCount);
               }
       */
        AliveNumCheck();

        uIMgr.MyRedSoul(redCount);
    }
    public void GetBuleSoul()
    {
        blueCount++;
        uIMgr.MyBlueSoul(blueCount);
        if (blueCount >= 5)
        {
            uIMgr.photonView.RPC("EndGame", RpcTarget.All, 2, blueCount);
        }
    }
    public GameObject PunFindObject(int viewID3)//뷰아이디를 넘겨받아 포톤상의 오브젝트를 찾는다.
    {
        GameObject find = null;
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < viewObject.Length; i++)
        {
            if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
        }
        return find;
    }
    public void DestroyTarget(GameObject desObject, float time)
    {
        photonView.RPC("PunDestroyObject", RpcTarget.All, desObject.GetPhotonView().ViewID, time);
    }
    [PunRPC]
    public void PunDestroyObject(int viewid, float time)
    {
        Destroy(PunFindObject(viewid), time);
    }


    public void AliveNumCheck()
    {
        //생존 인원 카운트 숫자
        alivePlayerNum = 0;
        //플레이어 인포를 찾기위한 배열 
        PlayerInfo[] AliveNum;
        int winner =0;



        AliveNum = FindObjectsOfType<PlayerInfo>();
        //상태가 Die가 아니라면 살아있는 것이기 때문에 살아남은 인원 카운트 가능 
        for (int i = 0; i < AliveNum.Length; i++)
        {
            if (AliveNum[i].playerState != state.Die)
            {
                alivePlayerNum++;
                winner = i;
            }
        }
        Debug.Log("살아남은 플레이어 수 = " + alivePlayerNum);

        if (alivePlayerNum == 1)
        {
            uIMgr.photonView.RPC("EndGame", RpcTarget.All, 1, AliveNum[winner].gameObject.GetPhotonView().ViewID);
        }
    }
}
