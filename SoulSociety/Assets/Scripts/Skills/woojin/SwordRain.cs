using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordRain : MonoBehaviourPun
{

    bool skillCool = false;
    bool skillClick = false;
    ResourceData eff;


    public void ResetCooltime()
    {
        skillCool = false;//��ų�� �ٽ� ��� �����ϰ���
        Debug.Log("��ų��");
    }
    public void SkillFire()
    {
        if (skillCool == false)
        {
            if (skillClick == false)
            {



                skillClick = true;
            }
            else skillClick = false;
        }
    }
    public void SkillClick(Vector3 Pos)
    {

        if (skillClick == true)
        {

            RaycastHit hit;
            Vector3 desiredDir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Pos);
            int mask = 1 << LayerMask.NameToLayer("Terrain");
            Physics.Raycast(Camera.main.ScreenPointToRay(Pos), out hit, 30f, mask);

            Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 1f);

            if (hit.collider.tag == "Ground")
            {
                desiredDir = hit.point;
                desiredDir.y = transform.position.y;
            }
            if (skillCool == false)//��ų ��� �����̸�
            {
                GameObject a = PhotonNetwork.Instantiate("SwordRain", desiredDir, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
                a.AddComponent<SkillHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
                a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.
                                                                                                                         // a.transform.LookAt(desiredDir);
                                                                                                                         // a.transform.Translate(0, 1, 0);
                a.transform.Rotate(-90, 0, 0);

                Destroy(a, 4f);
                // StartCoroutine(Fire(a));//ť�� �̵���Ű�� �ڷ�ƾ
                skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
                skillClick = false;
                Debug.Log("��ų���");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 10);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
            }
        }


    }
    IEnumerator Fire(GameObject skill)//ť�� �̵���Ű��
    {
        //skill.transform.position = new Vector3(0, 0, 0);
        // skill.transform.rotation = new Quaternion(0, 180, 0, 1);


        yield return null;
    }
}