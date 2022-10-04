using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilEyeCh : MonoBehaviourPun
{
    GameObject trapEff;
    int Attacker;//������ ����

    void AttackerName(int Name)//����޼����� ���� ��ID�� �Ѱܹ޴´�.
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

            GameObject a = PhotonNetwork.Instantiate("DevilEye", transform.position, Quaternion.identity);//��������Ʈ
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 15f, Attacker, state.Stun, 2.5f);

            a.transform.Rotate(-90f, 0f, 0f);

            Destroy(trapEff, 3f);
            GameMgr.Instance.DestroyTarget(a, 3f);
            GameMgr.Instance.DestroyTarget(gameObject, 3f);

        }
    }

}
