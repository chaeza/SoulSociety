using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using Photon.Pun;

public class Skill_1chance : MonoBehaviourPun, ItemMethod
{
    Animator myAnimator;
    NavMeshAgent navMeshAgent;
    PlayerMove playerMove;

    float dashSpeed = 20;
    SkillState skillState;
    Vector3 desiredDir;
    Vector3 clickPos = Vector3.one;

    int itemNum;

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
                myAnimator.SetTrigger("isBasicDash");
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(desiredDir);
            }
            else
               // photonView.RPC("DashStop", RpcTarget.AllBuffered);
            DashStop();
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
            navMeshAgent.speed = dashSpeed;
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
       // photonView.RPC("DashStop", RpcTarget.AllBuffered);
        DashStop();
    }
    //멈춤 조건
    
    public void DashStop()
    {
        navMeshAgent.speed = dashSpeed / 4;
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


//인터페이스

    public void ItemFire()
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }

    public void ItemSkill()
    {
        DashFire();

        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<Skill_1chance>(),0.3f);
    }

    public void GetItem(int itemnum)
    {
        if (itemNum == 0)
            itemNum = itemnum;
    }
}
