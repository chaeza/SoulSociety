using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using Photon.Pun;

public class Skill_basicDash : MonoBehaviourPun
{
    bool skillCool = false;
    Animator myAnimator;
    NavMeshAgent navMeshAgent;

    float dashSpeed = 15;
    Vector3 desiredDir;
    Vector3 clickPos = Vector3.one;

    public void ResetCooltime2()
    {
        skillCool = false;//스킬을 다시 사용 가능하게함
        Debug.Log("스킬쿨끝");
    }
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        skillCool = false;
    }
    public void DashFire()
    {
        if (skillCool == false)
        {   
            skillCool = true;
            myAnimator.SetTrigger("isBasicDash");
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(desiredDir);
            clickPos = Input.mousePosition;
            clickPos.z = 18f;
            navMeshAgent.speed = dashSpeed;

            GameMgr.Instance.uIMgr.DashCooltime(gameObject, 5);//UI매니저에 쿨타임 10초를 보냄
            StartCoroutine(DashTimer());
        } 
    }

    //대쉬 실행 시간
    IEnumerator DashTimer()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.03f);
            navMeshAgent.speed = dashSpeed;
            yield return null;
        }
        navMeshAgent.speed = dashSpeed / 4;
        navMeshAgent.isStopped = true;
        yield break;
    }
}
