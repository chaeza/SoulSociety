using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WindCycloneHit : MonoBehaviourPun
{
    int Attacker;//������ ����
    float timer;

    void AttackerName(int Name)//����޼����� ���� ��ID�� �Ѱܹ޴´�.
    {
        Attacker = Name;
    }
    List<GameObject> attackList = new List<GameObject>();//���ݹ��� ������ ���� ����Ʈ�߰�.

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        if (timer > 0.5f && other.tag == "Player")
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 3f, Attacker, state.None, 0f);//���������� �������� �ְ� ���� ������ ����.

            timer = 0f;
        }
    }
}
