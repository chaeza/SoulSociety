using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DevilSword : MonoBehaviourPun, SkillMethod
{
    int skillRange = 10;
    bool skillCool = false;
    bool skillClick = false;
    ResourceData eff;

    RectTransform myskillRangerect = null;
    GameObject skilla;
    GameObject skillCh;
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
        if (skillCool == false)//스킬 사용 가능이면
        {
            GameObject a = PhotonNetwork.Instantiate("DevilSword", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
            a.AddComponent<DevilSwordHit>();//이펙트에 히트 스크립트를 넣습니다.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.

            // a.transform.LookAt(desiredDir);
            //  a.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);

            skillCh = a;

            GameMgr.Instance.DestroyTarget(a, 1f);    //지속 시간

            //StartCoroutine(Sound());
            // a.transform.rotation = Quaternion.identity;

            skillCool = true;//쿨타임 온 시켜 다시 사용 못하게함

            Debug.Log("스킬사용");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 20);//UI매니저에 쿨타임 x초를 보냄
        }
    }
    
    
    
}
