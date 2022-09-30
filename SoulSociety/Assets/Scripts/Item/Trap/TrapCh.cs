using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TrapCh : MonoBehaviourPun
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "mainPlayer" && other.tag == "Player")
        {
             
            GameObject a = PhotonNetwork.Instantiate("Trap", transform.position, Quaternion.identity);//Æø¹ßÀÌÆåÆ®
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, gameObject.GetPhotonView().ViewID,state.Stun,2f);

            GameMgr.Instance.DestroyTarget(a, 2f);
            GameMgr.Instance.DestroyTarget(gameObject, 2f);
            
        }
    }
}
