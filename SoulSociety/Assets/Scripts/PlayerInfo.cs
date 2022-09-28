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
        End
    }
public class PlayerInfo : MonoBehaviourPun
{
    [SerializeField] int blueSoul = 0;
    [SerializeField] int redSoul = 0;
    [SerializeField] float maxHP = 100;
    private float curHP = 100;
    [SerializeField] float HPrecovery = 0.5f;
    [SerializeField] float basicAttackDamage = 10;

    HpBarInfo myHPbarInfo = null;

    RectTransform myrect = null;
    Animator myAnimator;
    GameObject myHit;
    int myNum = 0;
    [field:SerializeField] public state playerState { get; set; } = state.None;//�÷��̾� ����

    private void Start()
    {
        myHPbarInfo = GetComponentInChildren<HpBarInfo>();
        myHPbarInfo.SetName(photonView.Controller.NickName);

        transform.Find("SkillRange").gameObject.SetActive(false);
        

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


}
