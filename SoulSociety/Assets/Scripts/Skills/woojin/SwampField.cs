using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwampField : MonoBehaviourPun , SkillMethod
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
            GameObject a = PhotonNetwork.Instantiate("SwampField", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
            a.AddComponent<SwampHIT>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.

            // a.transform.LookAt(desiredDir);
            a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
            a.transform.Rotate(-90f, 0f, 0f);

            sound = a.GetComponent<AudioSource>();
            StartCoroutine(soundCh());
            sound.Play();

            GameMgr.Instance.DestroyTarget(a, 8f);

            skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
                             // skillClick = false;
            Debug.Log("��ų���");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 18);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
        }
    }
    IEnumerator soundCh()
    {
        yield return new WaitForSeconds(7f);
        sound.Stop();
    }
}
