using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Dodrug : MonoBehaviourPun , SkillMethod
{
    AudioSource sound;
    GameObject skilla;
    NavMeshAgent navMeshAgent;
    RectTransform myskillRangerect = null;
    
    Vector3 canSkill;
    Vector3 desiredDir;

    bool dashAttack = false;
    bool skillCool = false;
    bool skillClick = false;  
    int skillRange = 25;
   

    private void Start()
    {
        skilla = GetComponent<PlayerInfo>().skilla;
        myskillRangerect = GetComponent<PlayerInfo>().myskillRangerect;
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        if (dashAttack == true)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
            navMeshAgent.updatePosition = true;

            navMeshAgent.SetDestination(desiredDir);
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
            desiredDir = Vector3.zero;
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
                GetComponent<Animator>().SetTrigger("isSkill1");
                transform.LookAt(desiredDir);
                GetComponent<PlayerInfo>().Stay(0.5f);
                StartCoroutine(Stay(desiredDir, 0.2f));
                skillCool = true;//쿨타임 온 시켜 다시 사용 못하게함
                Debug.Log("스킬사용");
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 15);//UI매니저에 쿨타임 10초를 보냄
            }
        }
    }
    IEnumerator Stay(Vector3 desiredDir, float time)
    {
        yield return new WaitForSeconds(time);
        navMeshAgent.speed = 40f;
        dashAttack = true;
      //  GameObject a = PhotonNetwork.Instantiate("Dodrug", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        GameObject b = PhotonNetwork.Instantiate("DodrugHitBox", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        b.AddComponent<DodrugHit>();//이펙트에 히트 스크립트를 넣습니다.
        b.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.
        b.transform.LookAt(desiredDir);
        b.transform.Rotate(0, -180, 0);
       
        StartCoroutine(Fire(b));
      //  sound = a.GetComponent<AudioSource>();
      //  sound.Play();

        GameMgr.Instance.DestroyTarget(b, 1f);
        yield return new WaitForSeconds(0.5f);
        dashAttack = false;
        navMeshAgent.speed = 5f;

        yield break;
    }
    IEnumerator Fire(GameObject skill)  //큐브 이동시키기
    {
        for (int i = 0; i < 20; i++)
        {
            skill.transform.Translate(0, 0, -0.5f);
            yield return null;
        }
        yield break;
    }


}
