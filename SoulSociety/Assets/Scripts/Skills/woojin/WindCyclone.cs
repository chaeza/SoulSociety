using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WindCyclone : MonoBehaviourPun, SkillMethod
{
    bool skillCool = false;

    AudioSource sound;

    public void ResetCooltime()
    {
        skillCool = false;//��ų�� �ٽ� ��� �����ϰ���
        Debug.Log("��ų��");
    }

    public void SkillFire()
    {
        if (skillCool == false)//��ų ��� �����̸�
        {
            GameObject a = PhotonNetwork.Instantiate("WindCyclone", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
            a.AddComponent<WindCycloneHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.
            a.AddComponent<MyPosition>();
            a.transform.Rotate(-90f, 0f, 0f);
            a.SendMessage("MyPos", gameObject.transform, SendMessageOptions.DontRequireReceiver);
            // a.transform.LookAt(desiredDir);
            //  a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);

           

            GameMgr.Instance.DestroyTarget(a, 8f);    //���� �ð�

            //StartCoroutine(Sound());
            // a.transform.rotation = Quaternion.identity;

            skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
 
            Debug.Log("��ų���");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 20);//UI�Ŵ����� ��Ÿ�� x�ʸ� ����
        }
    }

    IEnumerator Sound()
    {
        sound.mute = false;

        yield return new WaitForSeconds(5f);

        sound.mute = true;
    }
}
