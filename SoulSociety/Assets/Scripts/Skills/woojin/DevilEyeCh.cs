using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilEyeCh : MonoBehaviourPun
{
    GameObject trapEff;
    int Attacker;//������ ����
    AudioSource sound;

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

            GameObject a = PhotonNetwork.Instantiate("DevilEye", transform.position, Quaternion.Euler(-90f,0f,0f));//��������Ʈ
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 15f, Attacker, state.Stun, 2.5f);

            sound = a.GetComponent<AudioSource>();
            StartCoroutine(soundCh());
            sound.Play();

            Destroy(trapEff, 6f);
            GameMgr.Instance.DestroyTarget(a, 7f);
            GameMgr.Instance.DestroyTarget(gameObject, 7f);

        }
    }

    IEnumerator soundCh()
    {
        yield return new WaitForSeconds(6f);
        sound.Stop();
    }
}
