using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
public class BoxPool : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "mainPlayer")
        {
            // gameObject.SetActive(false);

            SpawnMgr. spawnMgr.photonView.RPC("Relase", RpcTarget.All,gameObject);
            //spawnMgr.Relase(gameObject);
            GameMgr.Instance.randomItem.GetRandomitem(other.gameObject);
            //StartCoroutine("SpawnItem");  
        }
    }
}
