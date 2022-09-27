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
    [Header("[파란 영혼 수]")]
    [SerializeField] int blueSoul = 0; 
    [Header("[붉은 영혼 수]")]
    [SerializeField] int redSoul = 0;
    [Header("[최대 체력]")]
    [SerializeField] float HP = 100; 
    [Header("[기초 체력 재생]")]
    [SerializeField] float HPrecovery = 0.5f;
    [Header("[기본 공격력]")]
    [SerializeField] float basicAttackDamage = 10;
    Animator myAnimator;
    
    GameObject myHit;
    [field:SerializeField] public state playerState { get; set; } = state.None;//플레이어 상태 프로퍼티 

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        //플레이어 캐릭터가 자신의 플레이어라면 랜덤 스킬 함수를 통해 스킬 컴포넌트를 가져온다. 
        if (photonView.IsMine == true)
        {
            //자신의 플레이어 태그르 메인 플레이어로 변경, 트리거 사용을 위해서 
            gameObject.tag = "mainPlayer";
            //GameMgr을 통해서 랜덤 스킬컴포넌트를 배정 받음.
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
        }
    }

    //hitBox 콜라이더가 ON일 때
    private void OnTriggerEnter(Collider other)
    {
        //칼 콜라이더에 부딫힌 콜라이더 태그가 적이라면 PRC_hit 실행
        if (other.tag != "mainPlayer" && other.tag == "Player")
        {
            //ViewID 는 서버 접속한 플레이어들의 개별 ID이다. 
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All,basicAttackDamage,gameObject.GetPhotonView().ViewID);
        }
       
            Debug.Log("이름 : " + photonView.ViewID.ToString()+" , 부딫힌 대상: "+  other.tag.ToString());
      
    }
    //피격 함수
    [PunRPC]
    void RPC_hit(float bAD,int viewID1)
    {
        //Die 상태일 떄 체력 감소 x
        if (playerState==state.Die) return;
        HP -= bAD;
        //HP가 0이하로 떨어지면 죽음 함수 호출 
        if (HP <= 0)
            photonView.RPC("RPC_Die", RpcTarget.All,viewID1);
    }
    //죽음 함수
    [PunRPC]
    void RPC_Die(int viewID2)
    {
        //Die 상태일 떄 체력 감소 x
        if (playerState == state.Die) return;
        //자신이 죽은게 아닐 때만 GameMgr의 dieCount를 증가하는 함수 호출
        if (photonView.IsMine != true) GameMgr.Instance.UpdateDie();
        //죽은게 나라면 
        if (photonView.IsMine == true)
        {
            //날 죽인 플레이어의 ViewID 값으로 해당 UI에 붉은 영혼 수확 상태를 나타내고 GameMgr에 붉은 영혼 카운트 값을 올림 
            PunFindObject(viewID2).GetPhotonView().RPC("RPC_redSoul", RpcTarget.All, GameMgr.Instance.redCount);
            //붉은 영혼 카운트를 올림 
            GameMgr.Instance.uIMgr.MyRedSoul(0);
        }
        //플레이어 상대 죽음으로 전환 
        playerState = state.Die;
        //죽음 모션 플레이 
        myAnimator.SetTrigger("isDie");
        //태그 상태 전환
        gameObject.tag = "DiePlayer";
        //3초후 오브젝트 파괴
        Destroy(gameObject, 3f);
    }
    //붉은 영혼 UI 띄우기 
    [PunRPC]
    void RPC_redSoul(int redcount)
    {
        if (photonView.IsMine == true)
            GameMgr.Instance.GetRedSoul(redcount);
    }
    //파란 영혼 UI 띄우기
    public void BlueSoul()
    {
        if(GameMgr.Instance.redCount == 0)
        {
            GameMgr.Instance.GetBuleSoul();
        }
        else Debug.Log("빨간영혼을 얻으면 파란 영혼은 모을 수 없습니다.");
    }
    //자신을 죽인 플레이어의 오브젝트를 찾는 함수 
    GameObject PunFindObject(int viewID3)
    {
        //트리거를 통해 전달 받은 photonView ViewID 값을 가진 플레이어의 오브젝트를 찾는다. 
        GameObject find = null; 
        //배열로 받을 이유 있나..? 여러개일 경우가 있다??
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i<viewObject.Length;i++ )
        {
            if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
        }
        return find;
    }



}
