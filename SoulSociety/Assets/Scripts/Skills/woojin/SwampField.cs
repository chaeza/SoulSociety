using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwampField : MonoBehaviourPun , SkillMethod
{
    int skillRange = 10;
    bool skillCool = false;
    bool skillClick = false;
    ResourceData eff;
    RectTransform myskillRangerect = null;
    GameObject skilla;

    Vector3 canSkill;
    private void Start()
    {
        //��ų �����Ÿ��� ��ų��ȯ������ ����
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
                //D��ư�� �����¼��� �Ѵ� ����
                skilla.SetActive(true);
                myskillRangerect.gameObject.SetActive(true);
                //��ų �����Ÿ��̹����� SKILLRAGNE�� �޾� ũ�� ����
                myskillRangerect.sizeDelta = new Vector2(skillRange, skillRange);

                skillClick = true;
            }

            else skillClick = false;
        }
    }

    private void Update()
    {   //���� Ŭ����
        if (skillClick == true)
        {
            //���콺 ��ǥ�� �޾� ����
            Vector3 mousePos = Input.mousePosition;

            //Ÿ���� �ϳ� ����� ���콺 ��ġ�� �ѹ��� ����
            Vector3 target;
            target.x = mousePos.x;
            target.y = mousePos.y;
            target.z = 0;

            //��ȯ������ ��ġ�� Ÿ������ ����
            skilla.transform.position = target;

            RaycastHit hit;
            //����ĳ��ƮƲ ���� ��ų ���� �������� �Ǵ�
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 30f);
            canSkill = hit.point;
            canSkill.y = transform.position.y;
        }
    }
    public void SkillClick(Vector3 Pos)
    {
        if (skillClick == true)
        {
            if (Vector3.Distance(canSkill, transform.position) > skillRange / 2)
            {
                return;
            }
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
                GameObject a = PhotonNetwork.Instantiate("SwampField", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
                a.AddComponent<SkillHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
                a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.

               // a.transform.LookAt(desiredDir);
                a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
                a.transform.Rotate(-90f, 0f, 0f);

                GameMgr.Instance.DestroyTarget(a, 8f);



                skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
               // skillClick = false;
                Debug.Log("��ų���");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 10);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
            }
        }
    }

    IEnumerator Fire(GameObject skill)//ť�� �̵���Ű��
    {
        //skill.transform.position = this.transform.position + new Vector3(0, 0, 3);
        yield return null;
    }
}
