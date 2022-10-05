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
            sound = a.GetComponent<AudioSource>();
            StartCoroutine(soundCh());
            a.transform.Rotate(-90f, 0f, 0f);
            a.SendMessage("MyPos", gameObject.transform, SendMessageOptions.DontRequireReceiver);
            sound.Play();


            GameMgr.Instance.DestroyTarget(a, 8f);    //���� �ð�
            

            skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���

            Debug.Log("��ų���");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 20);//UI�Ŵ����� ��Ÿ�� x�ʸ� ����
        }
    }

    IEnumerator soundCh()
    {
        yield return new WaitForSeconds(7f);
        sound.Stop();
    }
}
