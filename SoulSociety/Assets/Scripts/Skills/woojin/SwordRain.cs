using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordRain : MonoBehaviourPun , SkillMethod
{
    int skillRange = 30;
    bool skillCool = false;
    bool skillClick = false;
    ResourceData eff;

    RectTransform myskillRangerect = null;
    GameObject skilla;

    Vector3 canSkill;
    private void Start()
    {
        myskillRangerect = GetComponentInChildren<SkillRange>().gameObject.GetComponent<RectTransform>();
        myskillRangerect.gameObject.SetActive(false);

        skilla = GameObject.Find("Skilla");
        skilla.SetActive(false);
    }

    public void ResetCooltime()
    {
        skillCool = false;//스킬을 다시 사용 가능하게함
        Debug.Log("스킬쿨끝");
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
            GetComponent<Animator>().SetTrigger("isAttack");
            GetComponent<PlayerInfo>().Stay(1f);
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
            if (skillCool == false)//스킬 사용 가능이면
            {
                GameObject a = PhotonNetwork.Instantiate("SwordRain", desiredDir, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
                a.AddComponent<SwordRainHIT>();//이펙트에 히트 스크립트를 넣습니다.
                a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.
                                                                                                                         // a.transform.LookAt(desiredDir);
                transform.LookAt(desiredDir);                                                                        // a.transform.Translate(0, 1, 0);
                a.transform.Rotate(-90, 0, 0);

                GameMgr.Instance.DestroyTarget(a, 4f);
                // StartCoroutine(Fire(a));//큐브 이동시키는 코루틴
                skillCool = true;//쿨타임 온 시켜 다시 사용 못하게함
                Debug.Log("스킬사용");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 22);//UI매니저에 쿨타임 10초를 보냄
            }
        }


    }
    IEnumerator Fire(GameObject skill)//큐브 이동시키기
    {
        //skill.transform.position = new Vector3(0, 0, 0);
        // skill.transform.rotation = new Quaternion(0, 180, 0, 1);


        yield return null;
    }
}