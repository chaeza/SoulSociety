using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    [SerializeField] float moveSpeed = 1;
    PlayerInfo playerInfo;
    Animator myAnimator;

    bool isMove = false;

    Vector3 moveTarget;
    Vector3 desiredDir;
    Vector3 targetDir;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameMgr.Instance.endGame == true) return;
        if (playerInfo.playerState == state.Die) return;
        if (playerInfo.playerState == state.Stun) return;
        if (photonView.IsMine == false) return;
        if (Input.mousePosition.x > 85 && Input.mousePosition.x < 170 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance) GameMgr.Instance.uIMgr.OnExplantionSkill(true);
        else GameMgr.Instance.uIMgr.OnExplantionSkill(false);
        if (Input.mousePosition.x > 680 && Input.mousePosition.x < 760 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(1) == false) GameMgr.Instance.uIMgr.OnExplantionItem(1, GameMgr.Instance.inventory.GetInventory(1));
        else if (Input.mousePosition.x > 845 && Input.mousePosition.x < 915 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(2) == false) GameMgr.Instance.uIMgr.OnExplantionItem(2, GameMgr.Instance.inventory.GetInventory(2));
        else if (Input.mousePosition.x > 1005 && Input.mousePosition.x < 1080 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(3) == false) GameMgr.Instance.uIMgr.OnExplantionItem(3, GameMgr.Instance.inventory.GetInventory(3));
        else if (Input.mousePosition.x > 1160 && Input.mousePosition.x < 1240 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(4) == false) GameMgr.Instance.uIMgr.OnExplantionItem(4, GameMgr.Instance.inventory.GetInventory(4));
        else GameMgr.Instance.uIMgr.OnExplantionItem(5, 0);

        if (Input.mousePosition != null&&GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse1)
        {
            Move(Input.mousePosition);
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

                gameObject.GetComponent<Rigidbody>().velocity = targetDir.normalized * moveSpeed;
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

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        int mask = 1 << LayerMask.NameToLayer("Terrain");
        Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 20f, mask);

        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 5f);

        if (hit.collider.tag == "Ground")
        {
            desiredDir = hit.point;
            desiredDir.y = transform.position.y;
            targetDir = desiredDir - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDir);
         //   Debug.Log(targetDir.ToString());
            isMove = true;
        }
    }

    public void MoveStop()
    {
        myAnimator.SetBool("isMove", false);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isMove = false;
        //Debug.Log(isMove.ToString());
    }
}
