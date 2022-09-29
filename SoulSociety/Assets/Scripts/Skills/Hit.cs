using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Hit : MonoBehaviourPun
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All,30f,gameObject.GetPhotonView().ViewID,state.None,0f);

        }
    }
}
