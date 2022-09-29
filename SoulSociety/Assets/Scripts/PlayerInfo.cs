using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using TMPro;

    [field: SerializeField] public enum state
    {
        None,
        Die,
        Stun,
        Unbeatable,//무적
        Slow,
        End
    }
public class PlayerInfo : MonoBehaviourPun
{
    [SerializeField] int blueSoul = 0;
    [SerializeField] int redSoul = 0;
    [SerializeField] float maxHP = 100;
    public float curHP { get; set; } = 100;
    [SerializeField] float HPrecovery = 0.5f;
    public float basicAttackDamage { get; set; } = 10;
    public float damageDecrease { get; set; } = 0; // 데미지 감소

    HpBarInfo myHPbarInfo = null;

    Animator myAnimator;
    GameObject myHit;
    int myNum = 0;
    [field:SerializeField] public state playerState { get; set; } = state.None;//플레이어 상태
    Coroutine RecoveryHp=null;
    Coroutine stunState = null;
    Coroutine slowState = null;
    private void Start()
    {
        myHPbarInfo = GetComponentInChildren<HpBarInfo>();
        myHPbarInfo.SetName(photonView.Controller.NickName);
        RecoveryHp  = StartCoroutine(HPRecovery());//체력회복 코루틴 실행
        myAnimator = GetComponent<Animator>();
        if (photonView.IsMine == true)
        {
            gameObject.tag = "mainPlayer";
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
        }
        if (photonView.IsMine == true)//시작 때 자기 자신의 번호를 저장합니다.
        {
            Player[] sortedPlayers = PhotonNetwork.PlayerList;
            for (int i = 0; i < sortedPlayers.Length; i++)
            {
                if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
                {
                    myNum = i;
                }
            }
        }
        photonView.RPC("ChangeColor", RpcTarget.All,myNum);//자기번호를 넘겨 플레이어의 색상을 모두에게 바꿉니다. 
        photonView.RPC("TabUpdate", RpcTarget.All,myNum,playerState,1,0);//자신의 번호를 넘겨 탭상태를 갱신합니다.
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "mainPlayer" && other.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All,basicAttackDamage,gameObject.GetPhotonView().ViewID,state.None,0f);
        }
        
    }
    [PunRPC]
    void RPC_hit(float bAD,int viewID1,state st,float time)
    {
        if (playerState == state.Die) return;
        if (playerState == state.Unbeatable) return;//무적일시 맞지 않음
        if(st==state.Stun)
        {
            playerState = state.Stun;
            if(stunState!=null) StopCoroutine(stunState);
            stunState= StartCoroutine(MyStun(time));
        }
        if(st==state.Slow)
        {
            if (slowState != null) StopCoroutine(slowState);
            slowState= StartCoroutine(MySlow(time,bAD));
        }
        if (st != state.Slow) curHP -= bAD* (1-damageDecrease);// 1에 데미지감소를 빼줘서 받는 데미지감소
        myHPbarInfo.SetHP(curHP, maxHP);
        if (curHP <= 0)
            photonView.RPC("RPC_Die", RpcTarget.All,viewID1);
    }
    [PunRPC]
    void RPC_Die(int viewID2)
    {

        if (playerState == state.Die) return;

        if (photonView.IsMine == true)
        {   
            if(stunState != null)
            StopCoroutine(stunState);
            StopCoroutine(RecoveryHp);
            GameMgr.Instance.PunFindObject(viewID2).GetPhotonView().RPC("RPC_redSoul", RpcTarget.All, GameMgr.Instance.redCount);

            GameMgr.Instance.uIMgr.MyRedSoul(0);
        }
        playerState = state.Die;
        myAnimator.SetTrigger("isDie");
        gameObject.tag = "DiePlayer";
        if (photonView.IsMine == true) photonView.RPC("TabUpdate", RpcTarget.All, myNum, state.Die, 1, 0);//자신의 번호를 넘겨 탭상태를 갱신합니다.
        if (photonView.IsMine == true) photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 2,0);//자신의 번호를 넘겨 탭상태를 갱신합니다.
        //Destroy(gameObject, 3f);
    }
    [PunRPC]
    void RPC_redSoul(int redcount)
    {
        if (photonView.IsMine == true)
        {
            GameMgr.Instance.GetRedSoul(redcount);
            photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 2, GameMgr.Instance.redCount);//자신의 번호를 넘겨 탭상태를 갱신합니다.
            photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 3, 0);//자신의 번호를 넘겨 탭상태를 갱신합니다.
        }
    }
    public void BlueSoul()
    {
        if(GameMgr.Instance.redCount == 0)
        {
            GameMgr.Instance.GetBuleSoul();
            photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 3, GameMgr.Instance.blueCount);//자신의 번호를 넘겨 탭상태를 갱신합니다.
        }
        else Debug.Log("빨간영혼을 얻으면 파란 영혼은 모을 수 없습니다.");
    }
    bool isattack;
    void att()
    {
       isattack = true;



        photonView.RPC("attack", RpcTarget.All, isattack);
    }
    [PunRPC]
    void ChangeColor(int Num)
    {
        if (photonView.IsMine)//색상바뀌는게 자신이면 자신의 고유번호로 색을바꿉니다.
        {
            if (myNum == 1)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.magenta;
            }
            else if (myNum == 2)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.green;
            }
            else if (myNum == 3)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.yellow;
            }
            else if (myNum == 4)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.white;
            }
        }
        else//자신이 아니면 포톤상 넘겨받은 번호로 색상을 바꿉니다.
        {
            if (Num == 1)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.magenta;
            }
            else if (Num == 2)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.green;
            }
            else if (Num == 3)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.yellow;
            }
            else if (Num == 4)
            {
                Renderer[] mat = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < mat.Length; i++)
                    mat[i].material.color = Color.white;
            }
        }
    }
    [PunRPC]
    void TabUpdate(int Num,state pstate,int Num2,int Num3)//Num은 자기 번호 Num2는 닉네임,레드,블루 영혼 구분 Num3은 블루,영혼 일시 그 갯수
    { 
        if (Num2 == 1)
        {
            if (photonView.IsMine)
                GameMgr.Instance.uIMgr.TabNickName(myNum, playerState);
            else
                GameMgr.Instance.uIMgr.TabNickName(Num, pstate);
        }
        else if(Num2==2)//죽여서 레드 얻었을 때
        {
            GameMgr.Instance.uIMgr.RedTabSoul(Num,Num3);
        }
        else if(Num2==3)
        {
            GameMgr.Instance.uIMgr.BlueTabSoul(Num, Num3);
        }
    }
    IEnumerator HPRecovery()
    {
        while(playerState!=state.Die)
        {
            yield return new WaitForSeconds(1f);
            if(curHP<=maxHP) curHP+=HPrecovery;
            myHPbarInfo.SetHP(curHP, maxHP);
            yield return null;
        }
    }
    IEnumerator MyStun(float time)
    {
        GameObject player = PhotonNetwork.Instantiate("Stun", transform.position, Quaternion.identity);
        GameMgr.Instance.DestroyTarget(player.GetPhotonView().ViewID, time);
        yield return new WaitForSeconds(time);
        playerState = state.None;

    }
    IEnumerator MySlow (float time,float slow)
    {
        GetComponent<PlayerMove>().ChageSpeed(GetComponent<PlayerMove>().moveSpeed);
        GameObject player = PhotonNetwork.Instantiate("Slow", transform.position, Quaternion.identity);
        GameMgr.Instance.DestroyTarget(player.GetPhotonView().ViewID,time);
        GetComponent<PlayerMove>().ChageSpeed(GetComponent<PlayerMove>().moveSpeed * (1 - (slow / 100)));
        yield return new WaitForSeconds(time);
        GetComponent<PlayerMove>().ChageSpeed(GetComponent<PlayerMove>().moveSpeed);
    }
}
