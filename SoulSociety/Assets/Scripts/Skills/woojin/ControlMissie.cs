using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlMissie : MonoBehaviourPun
{
    int skillRange = 150;
    bool skillCool = false;
    bool skillClick = false;
    ResourceData eff;
    AudioSource sound;

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

            if (hit.collider.tag == "Ground" || hit.collider.tag == "UnGround")
            {
                desiredDir = hit.point;
                desiredDir.y = transform.position.y;
            }
            if (skillCool == false)//��ų ��� �����̸�
            {
                GetComponent<Animator>().SetTrigger("isSkill2");
                transform.LookAt(desiredDir);
                GetComponent<PlayerInfo>().Stay(2.5f);
                StartCoroutine(Stay(desiredDir, 0.3f));
                // StartCoroutine(Fire(a));//ť�� �̵���Ű�� �ڷ�ƾ
                skillCool = true;//��Ÿ�� �� ���� �ٽ� ��� ���ϰ���
                Debug.Log("��ų���");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 44);//UI�Ŵ����� ��Ÿ�� 10�ʸ� ����
            }
        }


    }

    IEnumerator Stay(Vector3 desiredDir, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject a = PhotonNetwork.Instantiate("ControlMissie", desiredDir, Quaternion.identity);//����Ʈ�� ���� �ν��Ͻ��� �մϴ�.
        a.AddComponent<ControlMissieHit>();//����Ʈ�� ��Ʈ ��ũ��Ʈ�� �ֽ��ϴ�.
        a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//����Ʈ�� �����ڸ� �����մϴ�.
        a.transform.Rotate(-90, 0, 0);
        yield return new WaitForSeconds(4.5f);    //�ݶ��̴� Ŵ
        a.GetComponent<SphereCollider>().enabled = true;

        // sound = a.GetComponent<AudioSource>();
        //  StartCoroutine(soundCh());
        // sound.Play();


        GameMgr.Instance.DestroyTarget(a, 5f);  //5�ʵ� ����
        yield break;
    }


    IEnumerator soundCh()
    {
        yield return new WaitForSeconds(4f);
        sound.Stop();
    }
}
