using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordCrash : MonoBehaviourPun , SkillMethod
{
    int skillRange = 15;
    bool skillCool = false;
    bool skillClick = false;
    ResourceData eff;

    RectTransform myskillRangerect = null;
    GameObject skilla;

    Vector3 canSkill;
    private void Start()
    {
        myskillRangerect = GetComponent<PlayerInfo>().myskillRangerect;

        skilla = GetComponent<PlayerInfo>().skilla;
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

            else
            {
                skillClick = false;
                myskillRangerect.gameObject.SetActive(false);
                skilla.SetActive(false);
            }
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

            RaycastHit hit;

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
            
            skillClick = false;
            myskillRangerect.gameObject.SetActive(false);
            skilla.SetActive(false);
            if (Vector3.Distance(canSkill, transform.position) > skillRange / 2) return;

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
                GetComponent<Animator>().SetTrigger("isSkill2");
                transform.LookAt(desiredDir);
                GetComponent<PlayerInfo>().Stay(1f);
                StartCoroutine(Stay(desiredDir, 0.4f));


                skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
                Debug.Log("��ų���");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 15);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
            }
        }
    }
    IEnumerator Stay(Vector3 desiredDir, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject a = PhotonNetwork.Instantiate("SwordCrash", desiredDir, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
        a.AddComponent<SwordCrashHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
        a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.
        a.transform.LookAt(transform.position);
        a.transform.Rotate(0, 180, 0);
        StartCoroutine(Fire(desiredDir));
        GameMgr.Instance.DestroyTarget(a, 3f);  //4�ʵ� ����
        yield break;
    }

        IEnumerator Fire(Vector3 pos)//ť�� �̵���Ű��
    {
        yield return new WaitForSeconds(1f);
        GameObject hitbox = PhotonNetwork.Instantiate("SwordCrashHitbox", pos, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
        hitbox.AddComponent<SwordCrashHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
        hitbox.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.
        hitbox.transform.LookAt(transform.position);
        hitbox.transform.Translate(0, 0, -4f);
        GameMgr.Instance.DestroyTarget(hitbox, 1f);  //4�ʵ� ����
        yield return null;
        yield break;
    }
}
