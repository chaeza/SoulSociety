using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using Photon.Pun;

public class Skill_basicDash : MonoBehaviourPun
{
    Animator myAnimator;
    NavMeshAgent navMeshAgent;
    PlayerMove playerMove;

    float dashSpeed = 20;
    SkillState skillState;
    Vector3 desiredDir;
    Vector3 clickPos = Vector3.one;


    enum SkillState
    {
        Dash,
        CoolTime,
        IsCan,
    }

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        skillState = SkillState.IsCan;
        playerMove = GetComponent<PlayerMove>();
    }


    private void Update()
    {
        if (skillState == SkillState.Dash)
        {
            if (Vector3.Distance(desiredDir, transform.position) > 0.1f)
            {
                myAnimator.SetBool("isBasicDash",true);
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(desiredDir);
            }
            else
                photonView.RPC("DashStop", RpcTarget.AllBuffered, "isBasicDash");
            //DashStop();
        }
    }
    public void DashFire()
    {
        if (skillState == SkillState.IsCan)
        { 
            clickPos = Input.mousePosition;
            clickPos.z = 18f;
            navMeshAgent.speed = dashSpeed;
            playerMove.MoveStop();
            Dash(clickPos);
        }
     
    }

    //대쉬 실행 플레이어 어택에서 실행 
    public void Dash(Vector3 mousePos)
    {
        if (skillState != SkillState.IsCan) return;

        RaycastHit hit;

        int mask = 1 << LayerMask.NameToLayer("Terrain");
        Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 30f, mask);

        if (hit.collider.tag == "Ground")
        {
            navMeshAgent.speed = dashSpeed ;
            desiredDir = hit.point;
            desiredDir.y = transform.position.y;
            skillState = SkillState.Dash;

            StartCoroutine(DashTimer());
        }
    }

    //대쉬 실행 시간
    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(0.2f);
        photonView.RPC("DashStop", RpcTarget.AllBuffered, "isBasicDash");
        //DashStop();
    }
    //멈춤 조건
    [PunRPC]
    public void DashStop(string moti)
    {
        myAnimator.SetBool("isBasicDash", false);
        navMeshAgent.speed = dashSpeed/4;
        navMeshAgent.isStopped = true;
        skillState = SkillState.CoolTime;
        
        StartCoroutine(CoolTimeTimer());
    }
    //쿨타임 
    IEnumerator CoolTimeTimer()
    {
        yield return new WaitForSeconds(5);
        skillState = SkillState.IsCan;
    }

}
