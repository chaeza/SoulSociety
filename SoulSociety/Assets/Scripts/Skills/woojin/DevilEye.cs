using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilEye : MonoBehaviourPun, SkillMethod
{
    bool skillCool = false;
    GameObject TrapE;

    public void ResetCooltime()
    {
        skillCool = false;//��ų�� �ٽ� ��� �����ϰ���
        Debug.Log("��ų��");
    }
    public void SkillFire()
    {
        if (skillCool == false)//��ų ��� �����̸�
        {
            GameObject a = PhotonNetwork.Instantiate("DevilEyeTrigger", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
          //  a.AddComponent<SwampHIT>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.

            a.AddComponent<DevilEyeCh>();
            // a.transform.LookAt(desiredDir);
            a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
            a.transform.Rotate(-90f, 0f, 0f);
            TrapE = Instantiate(GameMgr.Instance.resourceData.myTrapEff, transform.position, Quaternion.identity);
            //  GameMgr.Instance.DestroyTarget(a, 8f);
            a.GetComponent<DevilEyeCh>().TrapEffInfo(TrapE);


            skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
                             // skillClick = false;
            Debug.Log("��ų���");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 20);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
        }
    }
}
