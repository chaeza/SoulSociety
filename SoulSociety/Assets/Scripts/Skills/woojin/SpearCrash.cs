using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SpearCrash : MonoBehaviour
{
    int skillRange = 10;
    bool skillCool = false;
    bool skillClick = false;
    ResourceData eff;

    RectTransform myskillRangerect = null;
    GameObject skilla;
    private void Start()
    {
        myskillRangerect = GetComponentInChildren<SkillRange>().gameObject.GetComponent<RectTransform>();
        myskillRangerect.gameObject.SetActive(false);

        skilla = GameObject.Find("Skilla");
        skilla.SetActive(false);
    }

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
                skilla.SetActive(true);
                myskillRangerect.gameObject.SetActive(true);
                myskillRangerect.sizeDelta = new Vector2(skillRange, skillRange);

                skillClick = true;
            }

            else skillClick = false;
        }
    }
    private void Update()
    {
        if (skillClick == true)
        {
             
            Vector3 mousePos = Input.mousePosition;

            Vector3 target;
            target.x = mousePos.x;
            target.y = mousePos.y;
            target.z = 0;

            skilla.transform.position = target;
           
        }
    }
    public void SkillClick(Vector3 Pos)
    {
        if (skillClick == true)
        {
            myskillRangerect.gameObject.SetActive(false);
            skilla.SetActive(false);

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
                GameObject a = PhotonNetwork.Instantiate("SpearCrash", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
                a.AddComponent<SkillHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
                a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.

                a.transform.LookAt(desiredDir);
                a.transform.position = gameObject.transform.position + new Vector3(3f,3f,0f);
               a.transform.Rotate(-100f, 0f, 0f);

                

                skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
                skillClick = false;
                Debug.Log("��ų���");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 10);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
            }
        }
    }
    //IEnumerator Fire(GameObject skill)//ť�� �̵���Ű��
    //{
    //    //skill.transform.position = this.transform.position + new Vector3(0, 0, 3);
    //    yield return null;
    //}
}
