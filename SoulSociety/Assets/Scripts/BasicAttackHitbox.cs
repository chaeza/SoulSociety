using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BasicAttackHitbox : MonoBehaviourPun
{
    int Attacker;//������ ����
    float basicDamage;
    void AttackerName(int Name)//����޼����� ���� ��ID�� �Ѱܹ޴´�.
    {
        Attacker = Name;
    }
    void AttackerDamage(float Damage)//����޼����� ���� ��ID�� �Ѱܹ޴´�.
    {
        basicDamage = Damage;
    }
    List<GameObject> attackList = new List<GameObject>();//���ݹ��� ������ ���� ����Ʈ�߰�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && attackList.Contains(other.gameObject) == false)//����Ʈ�� �ȵ���ִ� ���� ����.
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, basicDamage, Attacker, state.None, 0f);
            attackList.Add(other.gameObject);//���� ���� ���� ����Ʈ�� �־� �ݶ��̴��� ����ٰ� �ٽø´� ��츦 ������.
        }

    }
}
