using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwampHIT : MonoBehaviourPun
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
                other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 50f, Attacker, state.Slow, 0.2f);//���������� �������� �ְ� ���� ������ ����.
                other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 2.5f, Attacker, state.None, 0f);//���������� �������� �ְ� ���� ������ ����.

            timer = 0f;
        }
        

        //if (other.tag == "Player" && attackList.Contains(other.gameObject) == false)//����Ʈ�� �ȵ���ִ� ���� ����.
        //{

        //    attackList.Add(other.gameObject);//���� ���� ���� ����Ʈ�� �־� �ݶ��̴��� ����ٰ� �ٽø´� ��츦 ������.
        //}
    }
}
