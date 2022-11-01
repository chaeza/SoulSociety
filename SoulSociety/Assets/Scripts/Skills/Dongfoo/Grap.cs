using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grap : MonoBehaviour , SkillMethod
{
    [SerializeField]
    int itemNum = 0;
    int skillRange = 20;
    bool skillCool = false;
    bool skillClick = false;
    RectTransform mySkillRangeRect = null;
    GameObject skilla;

    Vector3 canSkill;
    // Start is called before the first frame update
    private void Start()
    {
        mySkillRangeRect = GetComponent<PlayerInfo>().myskillRangerect;

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
                mySkillRangeRect.gameObject.SetActive(true);
                mySkillRangeRect.sizeDelta = new Vector2(skillRange, skillRange);

                skillClick = true;
            }

            else
            {
                skillClick = false;
                mySkillRangeRect.gameObject.SetActive(false);
                skilla.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
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
            mySkillRangeRect.gameObject.SetActive(false);
            skilla.SetActive(false);
            if (Vector3.Distance(canSkill, transform.position) > skillRange / 2) return;

            RaycastHit hit;
            Vector3 desiredDir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Pos);
            int mask = 1 << LayerMask.NameToLayer("Terrain");
            Physics.Raycast(Camera.main.ScreenPointToRay(Pos), out hit, 30f, mask);


            if (hit.collider.tag == "Ground" || hit.collider.tag == "UnGround")
            {
                desiredDir = hit.point;
                desiredDir.y = transform.position.y;
            }
            if (skillCool == false)//��ų ��� �����̸�
            {
                //  GetComponent<Animator>().SetTrigger("isSkill2");
                transform.LookAt(desiredDir);
                //GetComponent<PlayerInfo>().Stay(0.1f);//�÷��̾� ���ð�
                StartCoroutine(Stay(desiredDir, 0.1f));//��ų�ߵ��ð�
                skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
                Debug.Log("��ų���");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 2);//UI�Ŵ����� ��Ÿ�� x�ʸ� ����
            }
        }
        IEnumerator Stay(Vector3 desiredDir, float time)
        {

            yield return new WaitForSeconds(time);
            GameObject a = PhotonNetwork.Instantiate("Grap", transform.position, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
            a.AddComponent<GrapHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.
           
            a.transform.LookAt(desiredDir);
            StartCoroutine(Fire(a));
            a.GetComponent<BoxCollider>().enabled = true;

            GameMgr.Instance.DestroyTarget(a, 1f);

            yield break;
        }

        IEnumerator Fire(GameObject skill)//ť�� �̵���Ű��
        {
            for (int i = 0; i < 20; i++)
            {
                skill.transform.Translate(0, 0, 0.5f);
                yield return null;
            }
            yield break;
        }
    }
}
