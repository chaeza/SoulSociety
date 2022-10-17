using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Skill_BasicMissile : MonoBehaviourPun , SkillMethod
{
    int skillRange = 20;
    bool skillCool = false;
    bool skillClick = false;

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
            if (skillCool == false)//스킬 사용 가능이면
            {
                GetComponent<Animator>().SetTrigger("isSkill2");
                transform.LookAt(desiredDir);
                GetComponent<PlayerInfo>().Stay(0.2f);//플레이어 대기시간
                StartCoroutine(Stay(desiredDir, 0.2f));//스킬발동시간
                skillCool = true;//쿨타임 온 시켜 다시 사용 못하게함
                Debug.Log("스킬사용");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 5);//UI매니저에 쿨타임 10초를 보냄
            }
        }
    }
    IEnumerator Stay(Vector3 desiredDir, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject a = PhotonNetwork.Instantiate("Skill_BasicMissile_orange", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        a.AddComponent<Skill_BasicMissileHit>();//이펙트에 히트 스크립트를 넣습니다.
        a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.
        a.transform.LookAt(desiredDir);
     //   a.transform.Rotate(0, 90, 0);

        GameMgr.Instance.DestroyTarget(a, 4f);

        yield break;
    }
   /* IEnumerator Fire(GameObject skill)//큐브 이동시키기
    {
        for (int i = 0; i < 100; i++)
        {
            skill.transform.Translate(0, 0, 0.1f);
            yield return null;
        }
        yield break;
    }*/
}