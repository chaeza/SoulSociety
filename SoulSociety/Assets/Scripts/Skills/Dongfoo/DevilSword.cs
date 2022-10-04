using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilSword : MonoBehaviourPun, SkillMethod
{
    bool skillCool = false;
    public void ResetCooltime()
    {
        skillCool = false;//��ų�� �ٽ� ��� �����ϰ���
        Debug.Log("��ų��");
    }
    public void SkillFire()
    {
        if (skillCool == false)//��ų ��� �����̸�
        {
            GameObject a = PhotonNetwork.Instantiate("DevilSword", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
            a.AddComponent<DevilSwordHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.

            // a.transform.LookAt(desiredDir);
            //  a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);

            GameMgr.Instance.DestroyTarget(a, 1f);    //���� �ð�

            //StartCoroutine(Sound());
            // a.transform.rotation = Quaternion.identity;

            skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���

            Debug.Log("��ų���");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 20);//UI�Ŵ����� ��Ÿ�� x�ʸ� ����
        }
    }
    
    
    
}
