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

    private void Start()
    {
        if (photonView.IsMine == true)
        {
            gameObject.tag = "mainPlayer";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "mainPlayer" && other.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, basicAttackDamage);
            myHit = other.gameObject;
        }
    }
    public void Win()
    {
        //UI ¶ç¿ò
        Debug.Log(gameObject.tag.ToString() + "À©!");
    }

    [PunRPC]
    void RPC_hit(float bAD)
    {
        HP-=bAD;
        Debug.Log(gameObject.tag.ToString()+"Ã¼·Â" + HP);

        if (HP <= 0)
            myHit.GetPhotonView().RPC("Die", RpcTarget.All);
    }
    [PunRPC]
    void Die()
    {
        GameMgr.Instance.UpdateDie();
        //myAnimator.SetTrigger("isDie");
        Destroy(gameObject,3f);
    }
   

    
}
