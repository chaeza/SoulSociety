using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    //히트박스 콜라이더
    [SerializeField] GameObject hitBox = null;
    Animator myAnimator;
    PlayerInfo playerInfo;
    bool isAttack = true;

    private void Start()
    {
        myAnimator =GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        //게임 종료, 플레이어 캐릭터 사망, 스턴, 상대의 플레이어일 경우 Update 사용 x
        if (GameMgr.Instance.endGame == true) return;
        if (playerInfo.playerState == state.Die) return;
        if (playerInfo.playerState == state.Stun) return;
        if (photonView.IsMine == false) return;
        //A키 입력시 기본 공격 실행 
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A)
        {
            Attack();
        }

        //아이템, 및 스킬 사용 
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Q) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.W) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.E) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.R) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.F) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);

        //아이템 및 영혼 습득 테스트용 버튼
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Alpha1) GameMgr.Instance.randomItem.GetRandomitem(gameObject);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Alpha2) gameObject.SendMessage("BlueSoul", SendMessageOptions.DontRequireReceiver);
    }
    [PunRPC]
    public void Attack()
    {
        //모션 
        if (isAttack==true)
        {
            isAttack = false;
            //모션 랜덤 설정 
            int motionNum = Random.Range(0, 3);
            switch (motionNum)
            {
                case 0:
                    myAnimator.SetTrigger("isAttack1");
                    break;
                case 1:
                    myAnimator.SetTrigger("isAttack2");
                    break;
                case 2:
                    myAnimator.SetTrigger("isAttack3");
                    break;
            }
            //코루틴으로 딜레이 생성
            photonView.StartCoroutine(AttackDelay());
        }
        //사운드
        //전달 함수 
    }
    //평타 딜레이 
    IEnumerator AttackDelay()
    {
        Debug.Log("공격");

        //콜라이더 온 
        //hitBox.SetActive(true) ;  
        hitBox.GetComponentInChildren<BoxCollider>().enabled = true;
        GetComponent<PlayerMove>().MoveStop();
        yield return new WaitForSeconds(1);
        //콜라이더 오프 
        //hitBox.SetActive(false) ;  
        hitBox.GetComponentInChildren<BoxCollider>().enabled =false;
        isAttack = true;
    }


}
