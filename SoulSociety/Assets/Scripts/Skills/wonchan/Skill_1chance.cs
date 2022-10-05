using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using Photon.Pun;

public class Skill_1chance : MonoBehaviourPun, ItemMethod
{
    Animator myAnimator;
    NavMeshAgent navMeshAgent;

    float dashSpeed = 15;
    Vector3 desiredDir;
    Vector3 clickPos = Vector3.one;
    bool use = false;
    int itemNum;
    AudioSource sound;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    //대쉬 실행 시간
    IEnumerator DashTimer2()
    {
        for(int i=0; i < 5; i++)
        { 
            yield return new WaitForSeconds(0.03f);
            navMeshAgent.speed = dashSpeed;
            yield return null;
        }
        navMeshAgent.speed = dashSpeed / 4;
        navMeshAgent.isStopped = true;
        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<Skill_1chance>());
        yield break;
    }

    //인터페이스

    public void ItemFire()
    {
        if (use == true) return;
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }

    public void ItemSkill()
    {
        use = true;
        myAnimator.SetTrigger("isBasicDash");
        navMeshAgent.speed = dashSpeed;
        GameObject a = PhotonNetwork.Instantiate("Dash", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        GameMgr.Instance.DestroyTarget(a, 1f);
        a.transform.LookAt(GetComponent<PlayerMove>().desiredDir);
        sound = a.GetComponent<AudioSource>();

        sound.Play();

        StartCoroutine(DashTimer2());
    }

    public void GetItem(int itemnum)
    {
        if (itemNum == 0)
            itemNum = itemnum;
    }
}
