using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System.IO;
    [field: SerializeField] public enum state
    {
        None,
        Die,
        Stun,
        End
    }
public class PlayerInfo : MonoBehaviourPun
{
    [SerializeField] int blueSoul = 0;
    [SerializeField] int redSoul = 0;
    [SerializeField] float maxHP = 100;
    [SerializeField] float HPrecovery = 0.5f;
    [SerializeField] float basicAttackDamage = 10;
    private float curHP = 100;

    HpBarInfo myHPbarInfo = null;

    Animator myAnimator;
    GameObject myHit;
    [field:SerializeField] public state playerState { get; set; } = state.None;//플레이어 상태
    private void Awake()
    {
        myHPbarInfo = GetComponentInChildren<HpBarInfo>();
        Debug.Log(myHPbarInfo);
    }

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (photonView.IsMine == true)
        {
            gameObject.tag = "mainPlayer";
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
        }
        myHPbarInfo.SetName(photonView.Controller.NickName);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "mainPlayer" && other.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All,basicAttackDamage,gameObject.GetPhotonView().ViewID);
        }
        
    }
    [PunRPC]
    void RPC_hit(float bAD,int viewID1)
    {
        if (playerState==state.Die) return;
        curHP -= bAD;
        myHPbarInfo.SetHP(curHP, maxHP);
        if (curHP <= 0)
            photonView.RPC("RPC_Die", RpcTarget.All,viewID1);
    }
    [PunRPC]
    void RPC_Die(int viewID2)
    {

        if (playerState == state.Die) return;

        if (photonView.IsMine != true) GameMgr.Instance.UpdateDie();
        if (photonView.IsMine == true)
        {
            PunFindObject(viewID2).GetPhotonView().RPC("RPC_redSoul", RpcTarget.All, GameMgr.Instance.redCount);
            GameMgr.Instance.uIMgr.MyRedSoul(0);
        }
        playerState = state.Die;
        myAnimator.SetTrigger("isDie");
        gameObject.tag = "DiePlayer";
        Destroy(gameObject, 3f);
    }
    [PunRPC]
    void RPC_redSoul(int redcount)
    {
        if (photonView.IsMine == true)
            GameMgr.Instance.GetRedSoul(redcount);
    }
    public void BlueSoul()
    {
        if(GameMgr.Instance.redCount == 0)
        {
            GameMgr.Instance.GetBuleSoul();
        }
        else Debug.Log("빨간영혼을 얻으면 파란 영혼은 모을 수 없습니다.");
    }
    GameObject PunFindObject(int viewID3)
    {
        GameObject find = null;
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i<viewObject.Length;i++ )
        {
            if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
        }
        return find;
    }



}
