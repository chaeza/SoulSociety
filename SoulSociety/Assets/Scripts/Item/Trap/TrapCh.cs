using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TrapCh : MonoBehaviourPun
{
    GameObject trapEff;
    int Attacker;//공격자 선언
    void AttackerName(int Name)//샌드메세지로 공격 뷰ID를 넘겨받는다.
    {
        Attacker = Name;
    }
    public void TrapEffInfo(GameObject tE)
    {
        trapEff=tE;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
             
            GameObject a = PhotonNetwork.Instantiate("Trap", transform.position, Quaternion.identity);//폭발이펙트
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, Attacker,state.Stun,2f);

            Destroy(trapEff,2f);
            GameMgr.Instance.DestroyTarget(a, 2f);
            GameMgr.Instance.DestroyTarget(gameObject, 2f); 
            
        }
    }
}
