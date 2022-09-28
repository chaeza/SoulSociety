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
            GameObject a = PhotonNetwork.Instantiate("Trap", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, gameObject.GetPhotonView().ViewID);
        }
    }
}
