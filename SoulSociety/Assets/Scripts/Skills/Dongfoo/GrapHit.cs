using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
public class GrapHit : MonoBehaviour
{
    int Attacker;//공격자 선언
    Vector3 pos; 

    private GameObject attackerPos;
    void AttackerName(int Name)//샌드메세지로 공격 뷰ID를 넘겨받는다.
    {
        Attacker = Name;
        attackerPos = GameMgr.Instance.PunFindObject(Attacker);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )//리스트에 안들어있는 적만 맞음.
        {
            pos = attackerPos.transform.position;
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, Attacker, state.Stun, 1f);
            other.gameObject.GetPhotonView().RPC("BackMove", RpcTarget.All, pos, 0.5f, 50);

            //transform.Translate(0, 0, -10f);
            
        }

    }
}
