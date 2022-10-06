using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BoxPool : MonoBehaviourPun
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
            GameMgr.Instance.randomItem.GetRandomitem(other.gameObject);
            // gameObject.SetActive(false);
            GameMgr.Instance.spawnMgr.photonView.RPC("FindItemInPool", RpcTarget.All, gameObject.GetPhotonView().ViewID,myNum);
            Debug.Log("æ∆¿Ã≈€ ∏‘¿Ω");
            //spawnMgr.Relase(gameObject);
          
            //StartCoroutine("SpawnItem");  
        }
    }
    void BlackHole()
    {
        GameMgr.Instance.spawnMgr.photonView.RPC("FindItemInPool", RpcTarget.All, gameObject.GetPhotonView().ViewID, myNum);
    }
}
