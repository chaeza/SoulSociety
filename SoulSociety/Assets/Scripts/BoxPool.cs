using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BoxPool : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "mainPlayer")
        {
            GameMgr.Instance.randomItem.GetRandomitem(other.gameObject);
            // gameObject.SetActive(false);
            GameMgr.Instance.spawnMgr.photonView.RPC("FindItemInPool", RpcTarget.All, gameObject.GetPhotonView().ViewID);
            Debug.Log("æ∆¿Ã≈€ ∏‘¿Ω");
            //spawnMgr.Relase(gameObject);
          
            //StartCoroutine("SpawnItem");  
        }
    }
}
