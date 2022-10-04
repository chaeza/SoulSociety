using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilEyeCh : MonoBehaviourPun
{
    GameObject trapEff;
    int Attacker;//공격자 선언

    void AttackerName(int Name)//샌드메세지로 공격 뷰ID를 넘겨받는다.
    {
        Attacker = Name;
    }
    public void TrapEffInfo(GameObject tE)
    {
        trapEff = tE;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            GameObject a = PhotonNetwork.Instantiate("DevilEye", transform.position, Quaternion.identity);//폭발이펙트
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 15f, Attacker, state.Stun, 2.5f);

            a.transform.Rotate(-90f, 0f, 0f);

            Destroy(trapEff, 3f);
            GameMgr.Instance.DestroyTarget(a, 3f);
            GameMgr.Instance.DestroyTarget(gameObject, 3f);

        }
    }

}
