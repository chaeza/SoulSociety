using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameMgr : Singleton<GameMgr>
{
    // public FollowCam followCam;
    [Tooltip("Game MGR followCam")]
    [field: SerializeField] public FollowCam followCam { get; private set; } = null;

    [Tooltip("Game MGR hpBarInfo")]
    [field: SerializeField] public HpBarInfo hpBarInfo { get; private set; } = null;

    [Tooltip("Game MGR inventory")]
    [field: SerializeField] public Inventory inventory { get; private set; } = null;

    [Tooltip("Game MGR Player Input")]
    [field: SerializeField] public PlayerInput playerInput { get; private set; } = null;
    [Tooltip("Game MGR randomItem")]
    [field: SerializeField] public RandomItem randomItem { get; private set; } = null;
    [Tooltip("Game MGR randomSkill")]
    [field: SerializeField] public RandomSkill randomSkill { get; private set; } = null;
    [Tooltip("Game MGR resourceData")]
    [field: SerializeField] public ResourceData resourceData { get; private set; } = null;
    [Tooltip("Game MGR spawnMgr")]
    [field: SerializeField] public SpawnMgr spawnMgr { get; private set; } = null;
    [Tooltip("Game MGR uIMgr")]
    [field: SerializeField] public UIMgr uiMgr { get; private set; } = null;

    public bool endGame { get; private set; } = false;
    public int dieCount = 0;
    public int redCount = 0;
    public int blueCount = 0;
    private int alivePlayerNum = 0;


    private void Awake()
    {
        resourceData = Resources.Load<ResourceData>("ResourceData");
        playerInput = gameObject.AddComponent<PlayerInput>();
        randomSkill = gameObject.AddComponent<RandomSkill>();
        randomItem = gameObject.AddComponent<RandomItem>();
        inventory = gameObject.AddComponent<Inventory>();
        followCam = FindObjectOfType<FollowCam>();
        spawnMgr = FindObjectOfType<SpawnMgr>();
        uiMgr = FindObjectOfType<UIMgr>();
    }
    public void GetRedSoul(int redSoul)
    {
        int num = 0;
        PlayerInfo[] playerinfo = FindObjectsOfType<PlayerInfo>();
        for (int i = 0; i < playerinfo.Length; i++)
        {
            if (playerinfo[i].playerState == state.Die) num++;
        }

        if (redSoul == 0) redCount++;
        else redCount += redSoul + 1;
        AliveNumCheck();
        uiMgr.MyRedSoul(redCount);
    }

    //When get a BlueSoul, count & EndGame Check
    public void GetBuleSoul()
    {
        blueCount++;

        uiMgr.MyBlueSoul(blueCount);

        if (blueCount >= 5) uiMgr.photonView.RPC("EndGame", RpcTarget.All, 2, blueCount);
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

    /// <summary>
    /// This Function will be used for destroy the object What you want.
    /// This Function need two para,
    /// </summary>
    /// <param name="destroyObject">This Object that will be destroyed</param>
    /// <param name="time">How much time do you need to Destroy</param>
    public void DestroyTarget(GameObject destroyObject, float time)
    {
        photonView.RPC("PunDestroyObject", RpcTarget.All, destroyObject.GetPhotonView().ViewID, time);
    }
    [PunRPC]
    public void PunDestroyObject(int viewID, float time)
    {
        Destroy(PunFindObject(viewID), time);
    }


    public void AliveNumCheck()
    {
        //생존 인원 카운트 숫자
        alivePlayerNum = 0;
        //플레이어 인포를 찾기위한 배열 
        PlayerInfo[] AliveNum;
        int winner = 0;



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
            uiMgr.photonView.RPC("EndGame", RpcTarget.All, 1, AliveNum[winner].gameObject.GetPhotonView().ViewID);
        }
    }
}
