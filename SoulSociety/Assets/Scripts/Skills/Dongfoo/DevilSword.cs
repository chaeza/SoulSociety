using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilSword : MonoBehaviourPun, SkillMethod
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
            GetComponent<Animator>().SetTrigger("isSkill2");
            GetComponent<PlayerInfo>().Stay(1f);
            StartCoroutine(Stay(1f));
        }
    }
    IEnumerator Stay(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject a = PhotonNetwork.Instantiate("DevilSword", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
        a.AddComponent<DevilSwordHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
        sound = a.GetComponent<AudioSource>();
        a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.

        sound.Play();
        // a.transform.LookAt(desiredDir);
        //  a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);

        GameMgr.Instance.DestroyTarget(a, 1f);    //���� �ð�

        //StartCoroutine(Sound());
        // a.transform.rotation = Quaternion.identity;

        skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���

        Debug.Log("��ų���");
        GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 20);//UI�Ŵ����� ��Ÿ�� x�ʸ� ����
        yield break;
    }
    
    
    
}
