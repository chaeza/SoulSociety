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

    public Vector3 desiredDir;
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
            myAnimator.SetBool("isMove", false);
            return;
        }
        if (photonView.IsMine == false) return;
        if (Input.mousePosition.x > 80 && Input.mousePosition.x < 210 && Input.mousePosition.y > 25 && Input.mousePosition.y < 185) GameMgr.Instance.uIMgr.OnExplantionSkill(true);
        else GameMgr.Instance.uIMgr.OnExplantionSkill(false);
        if (Input.mousePosition.x > 680 && Input.mousePosition.x < 760 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(1) == false) GameMgr.Instance.uIMgr.OnExplantionItem(1, GameMgr.Instance.inventory.GetInventory(1));
        else if (Input.mousePosition.x > 845 && Input.mousePosition.x < 915 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(2) == false) GameMgr.Instance.uIMgr.OnExplantionItem(2, GameMgr.Instance.inventory.GetInventory(2));
        else if (Input.mousePosition.x > 1005 && Input.mousePosition.x < 1080 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(3) == false) GameMgr.Instance.uIMgr.OnExplantionItem(3, GameMgr.Instance.inventory.GetInventory(3));
        else if (Input.mousePosition.x > 1160 && Input.mousePosition.x < 1240 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(4) == false) GameMgr.Instance.uIMgr.OnExplantionItem(4, GameMgr.Instance.inventory.GetInventory(4));
        else GameMgr.Instance.uIMgr.OnExplantionItem(5, 0);
        if (Input.mousePosition.x > 1280 && Input.mousePosition.x < 1400 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155) GameMgr.Instance.uIMgr.OnExplantionDash(true);
        else GameMgr.Instance.uIMgr.OnExplantionDash(false);

        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SendMessage("SkillClick", Input.mousePosition, SendMessageOptions.DontRequireReceiver);

        if (GameMgr.Instance.playerInput.inputKey2 == KeyCode.Mouse1)
        {
            clickPos = Input.mousePosition;
            clickPos.z = 18f;
            //if(Input.mousePosition.x > 200 && Input.mousePosition.x < 1800&& Input.mousePosition.y < 1050&& Input.mousePosition.y >50)
            if (donMove == false) Move(clickPos);
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
                navMeshAgent.updateRotation = true;
                navMeshAgent.updatePosition = true;

                navMeshAgent.SetDestination(desiredDir);
            }
            else
                MoveStop();
        }



    }
    public void Move(Vector3 mousePos)
    {
        // ?????? ??????????
        // ??????
        // ???? ?????? (?????? ????)
        RaycastHit hit;
        int mask = 1 << LayerMask.NameToLayer("Terrain");
        bool nullCheck = Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 9999, mask);

        bool nullCheckHit = (nullCheck) ? hit.transform.gameObject.CompareTag("Ground") : false;
        bool nullCheckHit2 = (nullCheck) ? hit.transform.gameObject.CompareTag("UnGround") : false;


        if (nullCheckHit == true || nullCheckHit2 == true)
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
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = false;
        isMove = false;
       // Debug.Log(isMove.ToString()+"???????");
    }
}