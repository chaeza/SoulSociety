using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SoulPool : MonoBehaviourPun
{
    int myNum;
    public void MyNum(int num)
    {
        myNum = num;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mainPlayer")
        {
            GameMgr.Instance.spawnMgr.photonView.RPC("FindSoulInPool", RpcTarget.All, gameObject.GetPhotonView().ViewID,myNum);
            // spawnMgr.SoulRelase(gameObject);
            other.gameObject.SendMessage("BlueSoul", SendMessageOptions.DontRequireReceiver);
            //StartCoroutine("SpawnItem");  
        }
    }
}
