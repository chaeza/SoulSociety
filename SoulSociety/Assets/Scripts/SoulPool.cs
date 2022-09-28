using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SoulPool : MonoBehaviourPun
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mainPlayer")
        {
            gameObject.SetActive(false);
            SpawnMgr.spawnMgr.photonView.RPC("SoulRelase", RpcTarget.All,gameObject);
           // spawnMgr.SoulRelase(gameObject);
            other.gameObject.SendMessage("BlueSoul", SendMessageOptions.DontRequireReceiver);
            //StartCoroutine("SpawnItem");  
        }
    }
}
