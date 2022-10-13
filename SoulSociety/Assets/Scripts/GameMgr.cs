using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameMgr : Singleton<GameMgr>

{
    [Tooltip("��������")]
    [SerializeField]
    public Skill_1chance PLA;

    // public FollowCam followCam;
    [Tooltip("Game MGR Player Input")]
    [field: SerializeField]
    public PlayerInput playerInput { get; private set; } = null;

    [Tooltip("Game MGR followCam")]
    [field: SerializeField]
    public FollowCam followCam { get; private set; } = null;

    [Tooltip("Game MGR inventory")]
    [field: SerializeField]
    public Inventory inventory { get; private set; } = null;

    [Tooltip("Game MGR randomSkill")]
    [field: SerializeField]
    public RandomSkill randomSkill { get; private set; } = null;

    [Tooltip("Game MGR randomItem")]
    [field: SerializeField]
    public RandomItem randomItem { get; private set; } = null;

    [Tooltip("Game MGR uIMgr")]
    [field: SerializeField]
    public UIMgr uIMgr { get; private set; } = null;

    [Tooltip("Game MGR hpBarInfo")]
    [field: SerializeField]
    public HpBarInfo hpBarInfo { get; private set; } = null;

    [Tooltip("Game MGR spawnMgr")]
    [field: SerializeField]
    public SpawnMgr spawnMgr { get; private set; } = null;

    [Tooltip("Game MGR resourceData")]
    [field: SerializeField]
    public ResourceData resourceData { get; private set; } = null;
    public bool endGame { get; private set; } = false;
    public int dieCount = 0;
    public int redCount = 0;
    public int blueCount = 0;
  //  int playerNum = 3;

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
    public GameObject PunFindObject(int viewID3)//����̵� �Ѱܹ޾� ������� ������Ʈ�� ã�´�.
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
        //���� �ο� ī��Ʈ ����
        alivePlayerNum = 0;
        //�÷��̾� ������ ã������ �迭 
        PlayerInfo[] AliveNum;
        int winner =0;



        AliveNum = FindObjectsOfType<PlayerInfo>();
        //���°� Die�� �ƴ϶�� ����ִ� ���̱� ������ ��Ƴ��� �ο� ī��Ʈ ���� 
        for (int i = 0; i < AliveNum.Length; i++)
        {
            if (AliveNum[i].playerState != state.Die)
            {
                alivePlayerNum++;
                winner = i;
            }
        }
        Debug.Log("��Ƴ��� �÷��̾� �� = " + alivePlayerNum);

        if (alivePlayerNum == 1)
        {
            uIMgr.photonView.RPC("EndGame", RpcTarget.All, 1, AliveNum[winner].gameObject.GetPhotonView().ViewID);
        }
    }
}
