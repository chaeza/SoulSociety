using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
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
    public float basicAttackDamage { get; set; } = 10;
    public float damageDecrease { get; set; } = 0; // 데미지 감소

    HpBarInfo myHPbarInfo = null;
    public bool stay { get; set; } = false;
    Animator myAnimator;
    GameObject myHit;
    int myNum = 0;
    public RectTransform myskillRangerect = null;
    public GameObject skilla;
    [field:SerializeField] public state playerState { get; set; } = state.None;//플레이어 상태
    NavMeshAgent navMeshAgent;
    Coroutine stunState = null;
    Coroutine slowState = null;
    Coroutine onUnbeatable = null;
    [PunRPC]
    void ChageHP(float hp)
    {
        curHP += hp;
        if(curHP>=maxHP)
            curHP = maxHP;
        myHPbarInfo.SetHP(curHP, maxHP);
        if (photonView.IsMine)
            GameMgr.Instance.uIMgr.SetHP(curHP, maxHP);
    }
    [PunRPC]
    void SetUnbeatable(float time)
    {
        if (onUnbeatable != null) StopCoroutine(onUnbeatable);
        onUnbeatable = StartCoroutine(OnUnbeatable(time));

    }
    [PunRPC]
    void SetDamageDecrpease(float value,float time)
    {
        StartCoroutine(OnDamageDecrease(value,time));

    }
    private void Start()
    {
        myHPbarInfo = GetComponentInChildren<HpBarInfo>();
        myHPbarInfo.SetName(photonView.Controller.NickName);
        myAnimator = GetComponent<Animator>();
        if (photonView.IsMine == true)
        {
            gameObject.tag = "mainPlayer";
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
            GameMgr.Instance.uIMgr.MyPlayerViewID(photonView.ViewID);
            myskillRangerect = GetComponentInChildren<SkillRange>().gameObject.GetComponent<RectTransform>();
            myskillRangerect.gameObject.SetActive(false);

            skilla = GameObject.Find("Skilla");
            skilla.SetActive(false);
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
    public void Stay(float time)
    {
        stay = true;
        GetComponent<PlayerMove>().MoveStop();
        StartCoroutine(StayMe(time));

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
        if (st != state.Slow)
        {
            curHP -= bAD * (1f - damageDecrease);// 1에 데미지감소를 빼줘서 받는 데미지감소
            myHPbarInfo.SetHP(curHP, maxHP);
            if(photonView.IsMine)
            GameMgr.Instance.uIMgr.SetHP(curHP, maxHP);
        }
        if (curHP <= 0)
            photonView.RPC("RPC_Die", RpcTarget.All,viewID1);
    }
    [PunRPC]
    void RPC_Die(int viewID2)
    {

        if (playerState == state.Die) return;

        if (photonView.IsMine == true)
        {
            if(stunState != null) StopCoroutine(stunState);
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
    #region 플레이어 상태 코루틴
    IEnumerator StayMe(float time)
    {
        yield return new WaitForSeconds(time);
        stay = false;
        yield break;
    }
    IEnumerator MyStun(float time)
    {
        GetComponent<PlayerMove>().MoveStop();
        GameObject player = PhotonNetwork.Instantiate("Stun", transform.position, Quaternion.identity);
        player.transform.Translate(0, 1, 0);
        if(time<1f)
            GameMgr.Instance.DestroyTarget(player, 1f);
        else 
        GameMgr.Instance.DestroyTarget(player, time);
        yield return new WaitForSeconds(time);
        playerState = state.None;
        yield break;

    }
    IEnumerator MySlow (float time,float slow)
    {
        GetComponent<PlayerMove>().ChageSpeed(GetComponent<PlayerMove>().moveSpeed);
        GameObject player = PhotonNetwork.Instantiate("Slow", transform.position, Quaternion.identity);
        player.transform.Translate(0, 1, 0);
        if (time < 1f)
            GameMgr.Instance.DestroyTarget(player, 1f);
        else
            GameMgr.Instance.DestroyTarget(player,time);
        GetComponent<PlayerMove>().ChageSpeed(GetComponent<PlayerMove>().moveSpeed * (1 - (slow / 100)));
        yield return new WaitForSeconds(time);
        GetComponent<PlayerMove>().ChageSpeed(GetComponent<PlayerMove>().moveSpeed);
        yield break;
    }
    IEnumerator OnUnbeatable(float time)
    {
        Debug.Log("무적시작");
        playerState = state.Unbeatable;
        yield return new WaitForSeconds(time);
        if (playerState == state.Unbeatable)
        {
            playerState = state.None;
            Debug.Log("무적끝");
        }
        yield break;
    }
    IEnumerator OnDamageDecrease(float value,float time)
    {
        Debug.Log("뎀감시작");
        damageDecrease += value;
        yield return new WaitForSeconds(time);
        damageDecrease -= value;
        Debug.Log("뎀감끝");
        yield break;
    }
    [PunRPC]
    void BackMove(Vector3 pos, float time, int speed)
    {
        StartCoroutine(backMove(pos, time, speed));
    }
    IEnumerator backMove(Vector3 pos, float time,int speed)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        navMeshAgent.isStopped = false;
        navMeshAgent.updateRotation = true;
        navMeshAgent.updatePosition = true;
        for(int i = 0; i < 50; i++)
        {
            navMeshAgent.SetDestination(pos);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        navMeshAgent.speed = 5;

        yield break;
    }
    #endregion
}
