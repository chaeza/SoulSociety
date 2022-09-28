using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.IO;
    [field: SerializeField] public enum state
    {
        None,
        Die,
        Stun,
        Unbeatable,//����
        Slow,
        End
    }
public class PlayerInfo : MonoBehaviourPun
{
    [SerializeField] int blueSoul = 0;
    [SerializeField] int redSoul = 0;
    [SerializeField] float HP = 100;
    [SerializeField] float HPrecovery = 0.5f;
    [SerializeField] float basicAttackDamage = 10;
    Animator myAnimator;
    GameObject myHit;
    int myNum = 0;
    [field:SerializeField] public state playerState { get; set; } = state.None;//�÷��̾� ����

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (photonView.IsMine == true)
        {
            gameObject.tag = "mainPlayer";
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
        }
        if (photonView.IsMine == true)//���� �� �ڱ� �ڽ��� ��ȣ�� �����մϴ�.
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
        photonView.RPC("ChangeColor", RpcTarget.All,myNum);//�ڱ��ȣ�� �Ѱ� �÷��̾��� ������ ��ο��� �ٲߴϴ�. 
        photonView.RPC("TabUpdate", RpcTarget.All,myNum,playerState,1,0);//�ڽ��� ��ȣ�� �Ѱ� �ǻ��¸� �����մϴ�.
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
        HP -= bAD;
        if (HP <= 0)
            photonView.RPC("RPC_Die", RpcTarget.All,viewID1);
    }
    [PunRPC]
    void RPC_Die(int viewID2)
    {

        if (playerState == state.Die) return;

        if (photonView.IsMine == true)
        {
            PunFindObject(viewID2).GetPhotonView().RPC("RPC_redSoul", RpcTarget.All, GameMgr.Instance.redCount);

            GameMgr.Instance.uIMgr.MyRedSoul(0);
        }
        playerState = state.Die;
        myAnimator.SetTrigger("isDie");
        gameObject.tag = "DiePlayer";
        if (photonView.IsMine == true) photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 2,0);//�ڽ��� ��ȣ�� �Ѱ� �ǻ��¸� �����մϴ�.
        //Destroy(gameObject, 3f);
    }
    [PunRPC]
    void RPC_redSoul(int redcount)
    {
        if (photonView.IsMine == true)
        {
            GameMgr.Instance.GetRedSoul(redcount);
            photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 1,0);//�ڽ��� ��ȣ�� �Ѱ� �ǻ��¸� �����մϴ�.
            photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 2, GameMgr.Instance.redCount);//�ڽ��� ��ȣ�� �Ѱ� �ǻ��¸� �����մϴ�.
            photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 3, 0);//�ڽ��� ��ȣ�� �Ѱ� �ǻ��¸� �����մϴ�.
        }
    }
    public void BlueSoul()
    {
        if(GameMgr.Instance.redCount == 0)
        {
            GameMgr.Instance.GetBuleSoul();
            photonView.RPC("TabUpdate", RpcTarget.All, myNum, playerState, 3, GameMgr.Instance.blueCount);//�ڽ��� ��ȣ�� �Ѱ� �ǻ��¸� �����մϴ�.
        }
        else Debug.Log("������ȥ�� ������ �Ķ� ��ȥ�� ���� �� �����ϴ�.");
    }
    GameObject PunFindObject(int viewID3)//����̵� �Ѱܹ޾� ������� ������Ʈ�� ã�´�.
    {
        GameObject find = null;
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i<viewObject.Length;i++ )
        {
            if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
        }
        return find;
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
        if (photonView.IsMine)//����ٲ�°� �ڽ��̸� �ڽ��� ������ȣ�� �����ٲߴϴ�.
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
        else//�ڽ��� �ƴϸ� ����� �Ѱܹ��� ��ȣ�� ������ �ٲߴϴ�.
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
    void TabUpdate(int Num,state pstate,int Num2,int Num3)//Num�� �ڱ� ��ȣ Num2�� �г���,����,��� ��ȥ ���� Num3�� ���,��ȥ �Ͻ� �� ����
    { 
        if (Num2 == 1)
        {
            if (photonView.IsMine)
                GameMgr.Instance.uIMgr.TabNickName(myNum, playerState);
            else
                GameMgr.Instance.uIMgr.TabNickName(Num, pstate);
        }
        else if(Num2==2)//�׿��� ���� ����� ��
        {
            GameMgr.Instance.uIMgr.RedTabSoul(Num,Num3);
        }
        else if(Num2==3)
        {
            GameMgr.Instance.uIMgr.BlueTabSoul(Num, Num3);
        }
    }
}
