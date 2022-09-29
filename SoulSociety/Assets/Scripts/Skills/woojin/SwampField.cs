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
        //스킬 사정거리와 스킬소환포인터 연결
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
                //D버튼이 눌리는순간 둘다 켜줌
                skilla.SetActive(true);
                myskillRangerect.gameObject.SetActive(true);
                //스킬 사정거리이미지를 SKILLRAGNE를 받아 크기 설정
                myskillRangerect.sizeDelta = new Vector2(skillRange, skillRange);

                skillClick = true;
            }

            else skillClick = false;
        }
    }

    private void Update()
    {   //아직 클릭전
        if (skillClick == true)
        {
            //마우스 좌표를 받아 저장
            Vector3 mousePos = Input.mousePosition;

            //타겟을 하나 만들어 마우스 위치를 한번더 저장
            Vector3 target;
            target.x = mousePos.x;
            target.y = mousePos.y;
            target.z = 0;

            //소환포인터 위치를 타겟으로 설정
            skilla.transform.position = target;

            RaycastHit hit;
            //레이캐스트틀 쏴서 스킬 가능 범위인지 판단
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
            if (skillCool == false)//스킬 사용 가능이면
            {
                GameObject a = PhotonNetwork.Instantiate("SwampField", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
                a.AddComponent<SkillHit>();//이펙트에 히트 스크립트를 넣습니다.
                a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.

               // a.transform.LookAt(desiredDir);
                a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
                a.transform.Rotate(-90f, 0f, 0f);

                GameMgr.Instance.DestroyTarget(a, 8f);



                skillCool = true;//쿨타임 온 시켜 다시 사용 못하게함
               // skillClick = false;
                Debug.Log("스킬사용");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 10);//UI매니저에 쿨타임 10초를 보냄
            }
        }
    }

    IEnumerator Fire(GameObject skill)//큐브 이동시키기
    {
        //skill.transform.position = this.transform.position + new Vector3(0, 0, 3);
        yield return null;
    }
}
