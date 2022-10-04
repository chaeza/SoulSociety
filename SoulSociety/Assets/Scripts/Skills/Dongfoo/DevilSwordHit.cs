using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilSwordHit : MonoBehaviourPun
{
    int Attacker;//공격자 선언
    void AttackerName(int Name)//샌드메세지로 공격 뷰ID를 넘겨받는다.
    {
        Attacker = Name;
    }
    List<GameObject> attackList = new List<GameObject>();//공격받은 적들을 넣을 리스트추가.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && attackList.Contains(other.gameObject) == false)//리스트에 안들어있는 적만 맞음.
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 30f, Attacker, state.None, 0f);
            attackList.Add(other.gameObject);//공격 받은 적을 리스트에 넣어 콜라이더를 벗어났다가 다시맞는 경우를 방지함.
        }

    }//}

}
