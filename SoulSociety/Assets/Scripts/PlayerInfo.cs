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
    [Header("[�Ķ� ��ȥ ��]")]
    [SerializeField] int blueSoul = 0; 
    [Header("[���� ��ȥ ��]")]
    [SerializeField] int redSoul = 0;
    [Header("[�ִ� ü��]")]
    [SerializeField] float HP = 100; 
    [Header("[���� ü�� ���]")]
    [SerializeField] float HPrecovery = 0.5f;
    [Header("[�⺻ ���ݷ�]")]
    [SerializeField] float basicAttackDamage = 10;
    Animator myAnimator;
    
    GameObject myHit;
    [field:SerializeField] public state playerState { get; set; } = state.None;//�÷��̾� ���� ������Ƽ 

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        //�÷��̾� ĳ���Ͱ� �ڽ��� �÷��̾��� ���� ��ų �Լ��� ���� ��ų ������Ʈ�� �����´�. 
        if (photonView.IsMine == true)
        {
            //�ڽ��� �÷��̾� �±׸� ���� �÷��̾�� ����, Ʈ���� ����� ���ؼ� 
            gameObject.tag = "mainPlayer";
            //GameMgr�� ���ؼ� ���� ��ų������Ʈ�� ���� ����.
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
        }
    }

    //hitBox �ݶ��̴��� ON�� ��
    private void OnTriggerEnter(Collider other)
    {
        //Į �ݶ��̴��� �΋H�� �ݶ��̴� �±װ� ���̶�� PRC_hit ����
        if (other.tag != "mainPlayer" && other.tag == "Player")
        {
            //ViewID �� ���� ������ �÷��̾���� ���� ID�̴�. 
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All,basicAttackDamage,gameObject.GetPhotonView().ViewID);
        }
       
            Debug.Log("�̸� : " + photonView.ViewID.ToString()+" , �΋H�� ���: "+  other.tag.ToString());
      
    }
    //�ǰ� �Լ�
    [PunRPC]
    void RPC_hit(float bAD,int viewID1)
    {
        //Die ������ �� ü�� ���� x
        if (playerState==state.Die) return;
        HP -= bAD;
        //HP�� 0���Ϸ� �������� ���� �Լ� ȣ�� 
        if (HP <= 0)
            photonView.RPC("RPC_Die", RpcTarget.All,viewID1);
    }
    //���� �Լ�
    [PunRPC]
    void RPC_Die(int viewID2)
    {
        //Die ������ �� ü�� ���� x
        if (playerState == state.Die) return;
        //�ڽ��� ������ �ƴ� ���� GameMgr�� dieCount�� �����ϴ� �Լ� ȣ��
        if (photonView.IsMine != true) GameMgr.Instance.UpdateDie();
        //������ ����� 
        if (photonView.IsMine == true)
        {
            //�� ���� �÷��̾��� ViewID ������ �ش� UI�� ���� ��ȥ ��Ȯ ���¸� ��Ÿ���� GameMgr�� ���� ��ȥ ī��Ʈ ���� �ø� 
            PunFindObject(viewID2).GetPhotonView().RPC("RPC_redSoul", RpcTarget.All, GameMgr.Instance.redCount);
            //���� ��ȥ ī��Ʈ�� �ø� 
            GameMgr.Instance.uIMgr.MyRedSoul(0);
        }
        //�÷��̾� ��� �������� ��ȯ 
        playerState = state.Die;
        //���� ��� �÷��� 
        myAnimator.SetTrigger("isDie");
        //�±� ���� ��ȯ
        gameObject.tag = "DiePlayer";
        //3���� ������Ʈ �ı�
        Destroy(gameObject, 3f);
    }
    //���� ��ȥ UI ���� 
    [PunRPC]
    void RPC_redSoul(int redcount)
    {
        if (photonView.IsMine == true)
            GameMgr.Instance.GetRedSoul(redcount);
    }
    //�Ķ� ��ȥ UI ����
    public void BlueSoul()
    {
        if(GameMgr.Instance.redCount == 0)
        {
            GameMgr.Instance.GetBuleSoul();
        }
        else Debug.Log("������ȥ�� ������ �Ķ� ��ȥ�� ���� �� �����ϴ�.");
    }
    //�ڽ��� ���� �÷��̾��� ������Ʈ�� ã�� �Լ� 
    GameObject PunFindObject(int viewID3)
    {
        //Ʈ���Ÿ� ���� ���� ���� photonView ViewID ���� ���� �÷��̾��� ������Ʈ�� ã�´�. 
        GameObject find = null; 
        //�迭�� ���� ���� �ֳ�..? �������� ��찡 �ִ�??
        PhotonView[] viewObject = FindObjectsOfType<PhotonView>();
        for (int i = 0; i<viewObject.Length;i++ )
        {
            if (viewObject[i].ViewID == viewID3) find = viewObject[i].gameObject;
        }
        return find;
    }



}
