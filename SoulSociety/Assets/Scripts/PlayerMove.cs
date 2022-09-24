using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    //플레이어 이동속도
    [SerializeField] float moveSpeed = 1;
    PlayerInfo playerInfo;
    Animator myAnimator;
    //이동 가능 상태 
    bool isMove = false;

    Vector3 moveTarget;
    //이동 목표 방향
    Vector3 desiredDir;
    //목표 방향으로의 방향 벡터 
    Vector3 targetDir;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //게임 종료, 플레이어 캐릭터 사망, 스턴, 상대의 플레이어일 경우 Update 사용 x
        if (GameMgr.Instance.endGame == true) return;
        if (playerInfo.playerState == state.Die) return;
        if (playerInfo.playerState == state.Stun) return;
        if (photonView.IsMine == false) return;

        //스킬 설명 UI
        if (Input.mousePosition.x > 85 && Input.mousePosition.x < 170 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance) GameMgr.Instance.uIMgr.OnExplantionSkill(true);
        else GameMgr.Instance.uIMgr.OnExplantionSkill(false);

        //아이템 설명 UI
        if (Input.mousePosition.x > 680 && Input.mousePosition.x < 760 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(1) == false) GameMgr.Instance.uIMgr.OnExplantionItem(1, GameMgr.Instance.inventory.GetInventory(1));
        else if (Input.mousePosition.x > 845 && Input.mousePosition.x < 915 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(2) == false) GameMgr.Instance.uIMgr.OnExplantionItem(2, GameMgr.Instance.inventory.GetInventory(2));
        else if (Input.mousePosition.x > 1005 && Input.mousePosition.x < 1080 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(3) == false) GameMgr.Instance.uIMgr.OnExplantionItem(3, GameMgr.Instance.inventory.GetInventory(3));
        else if (Input.mousePosition.x > 1160 && Input.mousePosition.x < 1240 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(4) == false) GameMgr.Instance.uIMgr.OnExplantionItem(4, GameMgr.Instance.inventory.GetInventory(4));
        else GameMgr.Instance.uIMgr.OnExplantionItem(5, 0);

        //마우스 우클릭입력을 받으면 Move함수로 이동 방향 설정 
        if (Input.mousePosition != null && GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse1)
        {
            Move(Input.mousePosition);
        }

        if (GameMgr.Instance.playerInput.inputKey == KeyCode.S)
        {
            MoveStop();
        }
        if (isMove == true)
        {
            //목표지점까지 이동
            if (Vector3.Distance(desiredDir, transform.position) > 0.1f)
            {
                myAnimator.SetBool("isMove", true);

                gameObject.GetComponent<Rigidbody>().velocity = targetDir.normalized * moveSpeed;
            }
            //목표지점까지 도착하면 멈춤
            else
                MoveStop();
        }
    }

    //우클릭 지점 방향 벡터 찾는 함수 
    public void Move(Vector3 mousePos)
    {
        // 움직임 애니메이션
        // 사운드
        // 실제 움직임 (포지션 변경)

        //레이캐스트를 통해 부딫힌 곳의 정보를 가져오기 위한 변수 
        RaycastHit hit;
        //마우스 포지션 값을 ScreenPointToRay로 변환해서 레이캐스트의 시작점 위치를 ray의 벡터로 넣어준다. 디버그로 레이캐스트를 보기위함
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        //비트 연산을 통해 레이어 Terrain레이어 마스크로 mask값을 변환 
        int mask = 1 << LayerMask.NameToLayer("Terrain");
        //mask의 레이어와 같은 곳에 충돌하는 레이캐스트를 발사한다. 
        Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 20f, mask);
        //레이캐스트를 보기 위한 디버그 
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 5f);

        //부딫힌 곳의 태그가 그라운드 일 경우만 이동 하게 하기 위함
        if (hit.collider.tag == "Ground")
        {
            //레이캐스트 충돌위치의 값을 저장한뒤 플레이어와 Y값을 동일하게 만들어 X,Z평면상에서의 방향벡터를 찾아낸다. 
            desiredDir = hit.point;
            desiredDir.y = transform.position.y;
            targetDir = desiredDir - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDir);
            //Debug.Log(targetDir.ToString());


            //움직일 수 있는 상태로 전환해준다. 
            isMove = true;
        }
    }
    // 움직임을 멈추는 함수
    public void MoveStop()
    {
        //움직임 상태와 velocity값을 0으로 만들어 움직임을 막는다. 
        myAnimator.SetBool("isMove", false);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isMove = false;
        //Debug.Log(isMove.ToString());
    }
}
