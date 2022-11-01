using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class KickHitbox : MonoBehaviourPun
{
    private int Attacker;//������ ����
    private Vector3 pos;
    private void AttackerName(int Name)//����޼����� ���� ��ID�� �Ѱܹ޴´�.
    {
        Attacker = Name;
    }
    List<GameObject> attackList = new List<GameObject>();//���ݹ��� ������ ���� ����Ʈ�߰�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && attackList.Contains(other.gameObject) == false)//����Ʈ�� �ȵ���ִ� ���� ����.
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, Attacker, state.Stun, 2f);
            transform.Translate(0, 0, 10f);
            pos = transform.position;
            transform.Translate(0, 0, -10f);
            other.gameObject.GetPhotonView().RPC("BackMove", RpcTarget.All,pos, 0.5f, 30);
            attackList.Add(other.gameObject);//���� ���� ���� ����Ʈ�� �־� �ݶ��̴��� ����ٰ� �ٽø´� ��츦 ������.
        }

    }
}