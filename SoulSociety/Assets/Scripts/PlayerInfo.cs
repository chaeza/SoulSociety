using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System.IO;
public class PlayerInfo : MonoBehaviourPun
{

    [SerializeField] float HP = 100;
    [SerializeField] float HPrecovery = 0.5f;
    [SerializeField] float basicAttackDamage = 10;
    Animator myAnimator;
    GameObject myHit;
    bool isDie = false;

    private void Start()
    {
        if (photonView.IsMine == true)
        {
            gameObject.tag = "mainPlayer";
            GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "mainPlayer" && other.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, basicAttackDamage);
        }
    }

    [PunRPC]
    void RPC_hit(float bAD)
    {
        if (isDie == true) return;
        HP -= bAD;
        Debug.Log(gameObject.tag.ToString() + "Ã¼·Â" + HP);
        if (HP <= 0)
            photonView.RPC("RPC_Die", RpcTarget.All);

    }
    [PunRPC]
    void RPC_Die()
    {
        if (isDie == true) return;
      
        if (photonView.IsMine != true)
            GameMgr.Instance.UpdateDie();
        //myAnimator.SetTrigger("isDie");
        Destroy(gameObject, 3f);
        gameObject.tag = "DiePlayer";
        isDie = true;
    }



}
