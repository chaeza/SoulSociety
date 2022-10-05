using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SlashHitbox : MonoBehaviourPun
{
    int Attacker;//������ ����
    void AttackerName(int Name)//����޼����� ���� ��ID�� �Ѱܹ޴´�.
    {
        Attacker = Name;
    }
    List<GameObject> attackList = new List<GameObject>();//���ݹ��� ������ ���� ����Ʈ�߰�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && attackList.Contains(other.gameObject) == false)//����Ʈ�� �ȵ���ִ� ���� ����.
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, Attacker, state.None, 0f);
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 50f, Attacker, state.Slow, 2f);
            attackList.Add(other.gameObject);//���� ���� ���� ����Ʈ�� �־� �ݶ��̴��� ����ٰ� �ٽø´� ��츦 ������.
        }

    }
}