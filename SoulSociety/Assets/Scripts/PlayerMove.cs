using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPun
{
    [SerializeField] public float moveSpeed { get; set; } = 5;
    PlayerInfo playerInfo;
    Animator myAnimator;
    NavMeshAgent navMeshAgent;

    bool isMove = false;
    public bool donMove { get; set; } = false;

    Vector3 desiredDir;
    Vector3 clickPos = Vector3.one;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        moveSpeed = 5;
        navMeshAgent.speed = moveSpeed;
    }
    public void ChageSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    private void Update()
    {
        if (playerInfo.stay == true) return;
        if (GameMgr.Instance.endGame == true) return;
        if (playerInfo.playerState == state.Die) return;
        if (playerInfo.playerState == state.Stun)
        {
            isMove = false;
            return;
        }
        if (photonView.IsMine == false) return;
        if (Input.mousePosition.x > 85 && Input.mousePosition.x < 170 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105) GameMgr.Instance.uIMgr.OnExplantionSkill(true);
        else GameMgr.Instance.uIMgr.OnExplantionSkill(false);
        if (Input.mousePosition.x > 680 && Input.mousePosition.x < 760 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(1) == false) GameMgr.Instance.uIMgr.OnExplantionItem(1, GameMgr.Instance.inventory.GetInventory(1));
        else if (Input.mousePosition.x > 845 && Input.mousePosition.x < 915 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(2) == false) GameMgr.Instance.uIMgr.OnExplantionItem(2, GameMgr.Instance.inventory.GetInventory(2));
        else if (Input.mousePosition.x > 1005 && Input.mousePosition.x < 1080 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(3) == false) GameMgr.Instance.uIMgr.OnExplantionItem(3, GameMgr.Instance.inventory.GetInventory(3));
        else if (Input.mousePosition.x > 1160 && Input.mousePosition.x < 1240 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(4) == false) GameMgr.Instance.uIMgr.OnExplantionItem(4, GameMgr.Instance.inventory.GetInventory(4));
        else GameMgr.Instance.uIMgr.OnExplantionItem(5, 0);
        if (Input.mousePosition.x > 310 && Input.mousePosition.x < 390 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105) GameMgr.Instance.uIMgr.OnExplantionDash(true);
        else GameMgr.Instance.uIMgr.OnExplantionDash(false);
      
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SendMessage("SkillClick",Input.mousePosition,SendMessageOptions.DontRequireReceiver);
      
        if (GameMgr.Instance.playerInput.inputKey2 == KeyCode.Mouse1)
        {
            clickPos = Input.mousePosition;
            clickPos.z = 18f;
            //if(Input.mousePosition.x > 200 && Input.mousePosition.x < 1800&& Input.mousePosition.y < 1050&& Input.mousePosition.y >50)
                if(donMove==false) Move(clickPos);
        }
      
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.S)
        {
            MoveStop();
        }

        if (isMove == true)
        {
            if (Vector3.Distance(desiredDir, transform.position) > 0.1f)
            {
                myAnimator.SetBool("isMove", true);
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(desiredDir);
            }
            else
                MoveStop();
        }
     
    }
    public void Move(Vector3 mousePos)
    {
        // 움직임 애니메이션
        // 사운드
        // 실제 움직임 (포지션 변경)
        RaycastHit hit;
        int mask = 1 << LayerMask.NameToLayer("Terrain");
        bool nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);

        bool nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;

        if (nullCheckHit==true)
        {
            desiredDir = hit.point;
            desiredDir.y = transform.position.y;
            isMove = true;
        }
    }
   
    public void MoveStop()
    {
        myAnimator.SetBool("isMove", false);
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        isMove = false;
        //Debug.Log(isMove.ToString());
    }
}